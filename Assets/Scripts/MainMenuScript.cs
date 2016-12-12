using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    void Start ()
    {
        // Reset player prefs on startup
        PlayerPrefs.SetInt("currentLives", 3);
        PlayerPrefs.SetInt("currentScore", 0);
        PlayerPrefs.SetInt("currentCoins", 0);
        PlayerPrefs.SetInt("finalScore", 00000);
        PlayerPrefs.SetInt("finalCoins", 00);
        PlayerPrefs.SetInt("finalTime", 000);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the player presses the Submit input, run the OnStart function
	    if (Input.GetButton("Submit"))
        {
            SceneManager.LoadScene("MainScene");
        }
        // If the player presses the Cancel input, quit the application
        else if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
	}
}
