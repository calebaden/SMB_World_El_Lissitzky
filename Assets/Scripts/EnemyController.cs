using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    GameManager gameManager;

    public LayerMask GroundLayers;

    private GameObject player;

    private int pointWorth = 100;
    private float destroyTime = 0.2f;
    private float moveSpeed = 3.0f;
    private int heading = 0;
    private Vector2 direction = Vector2.left;
    private bool inRange = false;
    private float aggroRange = 10.0f;
    private bool isAlive = true;

	// Use this for initialization
	void Start ()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If the inRange bool is true, move in the direction of the direction vector
        if (inRange && isAlive)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        // If the player enters the aggro range then set the inRange bool to true
        if (transform.position.x - player.transform.position.x < aggroRange && !inRange)
        {
            inRange = true;
        }
	}

    // Function that controls enemy death
    public void EnemyDeath (int multiplier)
    {
        gameManager.source.PlayOneShot(gameManager.stomp);
        isAlive = false;
        GetComponent<Collider2D>().enabled = false; // Disable the game objects collider
        GetComponent<Rigidbody2D>().gravityScale = 0; // Disable gravity
        // Switch the enabled child object from alive to dead
        transform.FindChild("SpriteAlive").gameObject.SetActive(false);
        transform.FindChild("SpriteDead").gameObject.SetActive(true);
        // Add points and destroy this game object
        gameManager.score += pointWorth * multiplier;
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter2D (Collision2D otherObject)
    {
        // If the enemy collides with layer 18 (void) then destroy it
        if (otherObject.collider.gameObject.layer == 18)
        {
            Destroy(gameObject);
        }

        // If the enemy collides with layer 16 (wall) then change the direction vector according to the current heading
        if (otherObject.collider.gameObject.layer == 16)
        {
            if (heading == 1)
            {
                direction = Vector2.left;
                heading = 0;
            }
            else if (heading == 0)
            {
                direction = Vector2.right;
                heading = 1;
            }
        }
    }
}
