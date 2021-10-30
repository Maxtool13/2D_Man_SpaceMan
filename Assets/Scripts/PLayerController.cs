using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PLayerController : MonoBehaviour
{

    public float jumpForce = 6f;
    public float RunningSped = 2f;

    Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;

    private const string STATE_ALIVE = "IsAlive";
    private const string STATE_ON_THE_GROUND = "IsOnTheGround";

    [SerializeField]
    private int healthPoints, manaPoints;

    public const int 
        INITIAL_HEALTH = 100, INITIAL_MANA = 10,
        MAX_HEALTH = 200, MAX_MANA = 50, 
        MIN_HELATH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;

    public const float SUPERJUMP_FORCE = 1.5f;

    public float jumpRaycastDistance = 1.5f;


    public LayerMask groundMask;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        

        startPosition = this.transform.position;
    }


    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.5f);

    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;

        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstace.currentGameState == GameState.inGame)
        {

            if (Input.GetButtonDown("Jump"))
            {
                Jump(false);
            }
            if (Input.GetButton("superjump"))
            {
                Jump(true);
            }
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        Debug.DrawRay(this.transform.position, Vector2.down * jumpRaycastDistance, Color.magenta);
    }

    private void FixedUpdate()
    {
        if(GameManager.sharedInstace.currentGameState == GameState.inGame) { 
            
            if(rigidBody.velocity.x < RunningSped)
            {
                    rigidBody.velocity = new Vector2(RunningSped, 
                        rigidBody.velocity.y);
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            
        }
    }
    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;

        if (superjump && manaPoints>=SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if (IsTouchingTheGround())
        {
            GetComponent<AudioSource>().Play();
            rigidBody.AddForce(Vector2.up * jumpForceFactor, 
                ForceMode2D.Impulse);
        }
    } 


    bool IsTouchingTheGround()
    {
        if(Physics2D.Raycast(this.transform.position,
            Vector2.down,
            jumpRaycastDistance, 
            groundMask)){
            
            return true;
        }else
        {
            
            return false;
        }

        
    }

    public void Die()
    {
        float travelledDistance = GetTravelDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);
        if(travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstace.GameOver();
    }


    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }

        if(this.healthPoints <= 0)
        {
            Die();
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }


    public int GetMana()
    {
        return manaPoints;  
    }

    public float GetTravelDistance()
    {
        return this.transform.position.x - startPosition.x;
    }


}
