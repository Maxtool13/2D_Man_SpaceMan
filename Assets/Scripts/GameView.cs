using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{

    public Text coinText, scoreText, maxScoreText;


    private PLayerController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").
            GetComponent<PLayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstace.currentGameState == GameState.inGame)
        {
            int coins = GameManager.sharedInstace.collectedObject;
            float score = controller.GetTravelDistance();
            float maxScore = PlayerPrefs.GetFloat("maxscore", 0f);

            coinText.text = coins.ToString();
            scoreText.text = "Score: " + score.ToString("f1");
            maxScoreText.text = "MaxScore: " + maxScore.ToString("f1");
        }
    }
}
