using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int lives = 3;
    public int coins = 0;
    public float timeLimit = 400.0f;
    public float timeRemaining;
    public int powerUpPoints = 1000;
    private float sceneWaitTime = 2.0f;
    public bool isPaused = false;

    public AudioSource source;

    public AudioClip coin;
    public AudioClip stomp;
    public AudioClip die;
    public AudioClip jump;
    public AudioClip powerup;

	// Use this for initialization
	void Start ()
    {
        timeRemaining = timeLimit;

        lives = PlayerPrefs.GetInt("currentLives");
        score = PlayerPrefs.GetInt("currentScore");
        coins = PlayerPrefs.GetInt("currentCoins");
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If cancel is press and the game is not paused, pause the game
        if (Input.GetButtonDown("Cancel") && !isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        // If cancel is pressed and the game is paused, exit to main menu
        else if (Input.GetButtonDown("Cancel") && isPaused)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        // If the submit button is pressed and the game is paused, resume the game
        if (Input.GetButtonDown("Submit") && isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
        }

        // Time decriments by delta time while above 0
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        // Calls PlayerDeath when time reaches or falls below 0
        else
        {
            if (lives <= 1)
            {
                StartCoroutine("GameOver");
            }
            else
            {
                StartCoroutine("PlayerDeath");
            }
        }
	}

    // Function that adds the parsed integer to the current score
    public void AddPoints (int points)
    {
        score += points;
    }

    // Custom coroutine that counts seconds outside of Time.time
    public static IEnumerator WaitForSecondsRealTime (float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    // When the player dies, subtract a life, set player prefs and restart
    public IEnumerator PlayerDeath ()
    {
        source.PlayOneShot(die);
        lives--;
        Time.timeScale = 0;
        PlayerPrefs.SetInt("currentLives", lives);
        PlayerPrefs.SetInt("currentScore", score);
        PlayerPrefs.SetInt("currentCoins", coins);
        PlayerPrefs.SetInt("gameOver", 0);
        yield return StartCoroutine(WaitForSecondsRealTime(sceneWaitTime));
        SceneManager.LoadScene("EndScene");
    }

    // When the player dies with no lives remaining, reset player prefs and restart
    public IEnumerator GameOver ()
    {
        source.PlayOneShot(die);
        Time.timeScale = 0;
        PlayerPrefs.SetInt("currentLives", 3);
        PlayerPrefs.SetInt("currentScore", 0);
        PlayerPrefs.SetInt("currentCoins", 0);
        PlayerPrefs.SetInt("gameOver", 1);
        yield return StartCoroutine(WaitForSecondsRealTime(sceneWaitTime));
        SceneManager.LoadScene("EndScene");
    }

    // When the player finishes the level, stop time, set player prefs and load the end scene
    public IEnumerator PlayerFinish ()
    {
        Time.timeScale = 0;
        PlayerPrefs.SetInt("finalScore", score);
        PlayerPrefs.SetInt("finalCoins", coins);
        PlayerPrefs.SetInt("finalTime", (int)timeRemaining);
        PlayerPrefs.SetInt("gameOver", 2);
        yield return StartCoroutine(WaitForSecondsRealTime(sceneWaitTime));
        SceneManager.LoadScene("EndScene");
    }

    public void OnGUI ()
    {

    }
}
