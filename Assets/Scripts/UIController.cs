using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    GameManager gameManager;

    public Text score;
    public Text coins;
    public Text stage;
    public Text time;
    public Text paused;
    public Text exitResume;

	// Use this for initialization
	void Start ()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    score.text = "MARIO" + "\n" + gameManager.score;
        coins.text = " x " + gameManager.coins;
        stage.text = "WORLD" + "\n" + "1-1";
        time.text = "TIME" + "\n" + (int)gameManager.timeRemaining;

        // If the game is paused, enable the pause text, otherwise, disable the pause text
        if (gameManager.isPaused)
        {
            paused.GetComponent<Text>().enabled = true;
            exitResume.GetComponent<Text>().enabled = true;
        }
        else
        {
            paused.GetComponent<Text>().enabled = false;
            exitResume.GetComponent<Text>().enabled = false;
        }
	}
}
