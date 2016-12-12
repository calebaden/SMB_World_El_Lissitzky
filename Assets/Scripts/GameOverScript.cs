using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverScript : MonoBehaviour {

    GameManager gameManager;

    private float lifeTime = 3.0f;
    private int gameResult;

    public int currentLives;
    public int currentScore;
    public int currentCoins;
    public Text livesLeft;
    public Text gameOver;
    public Text success;
    public Text score;
    public Text coins;
    public Text time;
    public Text exit;

	// Use this for initialization
	void Start ()
    {
        // Check if the time scale is 1 and set it accordingly
	    if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        gameResult = PlayerPrefs.GetInt("gameOver");

        // If gameResult is 0 (game over) then enable the game over text
        if (gameResult == 0)
        {
            livesLeft.GetComponent<Text>().enabled = true;
            livesLeft.text = "LIVES LEFT X " + PlayerPrefs.GetInt("currentLives");
        }
        // If gameResut is 1 (dead) then enable the lives left text
        else if (gameResult == 1)
        {
            gameOver.GetComponent<Text>().enabled = true;
        }
        // If gameResult is anything other than 0 or 1 (player finish) then enable the final stats text
        else
        {
            success.GetComponent<Text>().enabled = true;
            score.GetComponent<Text>().enabled = true;
            coins.GetComponent<Text>().enabled = true;
            time.GetComponent<Text>().enabled = true;
            exit.GetComponent<Text>().enabled = true;

            score.text = "Score: " + PlayerPrefs.GetInt("finalScore");
            coins.text = "Coins: " + PlayerPrefs.GetInt("finalCoins");
            time.text = "Time: " + PlayerPrefs.GetInt("finalTime");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (gameResult == 0 && Time.timeSinceLevelLoad > lifeTime)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (gameResult == 1 && Time.timeSinceLevelLoad > lifeTime)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (gameResult == 2 && Input.GetButton("Submit"))
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
