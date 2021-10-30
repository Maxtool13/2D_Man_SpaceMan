using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{

    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstace;

    private PLayerController controller;

    public int collectedObject = 0;

    void Awake()
    {
        if (sharedInstace == null)
        {
            sharedInstace = this;

        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").
            GetComponent<PLayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState !=GameState.inGame)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);

    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);


    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);

    }

    private void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            //TODO: colocar la logica del menu

            MenuManager.sharedInstance.ShowMainMenu();


        }else if(newGameState == GameState.inGame)
        {
            //TODO: Escena para jugar

            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlock();
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();

        }else if(newGameState == GameState.gameOver)
        {
            //TODO: preparar el game over

            MenuManager.sharedInstance.ShowMainMenu();



        }




        this.currentGameState = newGameState;
    }

    public void CollectObject (Collectable collectable)
    {
        collectedObject += collectable.value; 
    }
}
