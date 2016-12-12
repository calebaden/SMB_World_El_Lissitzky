using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    GameManager gameManager;

    private Transform groundCheck;
    public LayerMask GroundLayers;

    private float moveSpeed = 4.0f;
    private float acceleration = 0.0f;
    private float accelerationGain = 0.01f;
    private float maxAcceleration = 3.0f;
    private float jumpForce = 12.0f;
    private float stompForce = 300.0f;
    private bool isGrounded;
    private bool wasGrounded;
    private float jumpTimer;
    private float airForce = 1.5f;
    private float maxJump = 0.6f;
    private bool isPowered = false;
    public int stompMultiplier = 1;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        groundCheck = transform.FindChild("GroundCheck");
	}

    // FixedUpdate for physics based functions
    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") <0)
        {
            Movement();
        }

        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        isGrounded = Physics2D.OverlapPoint(groundCheck.position, GroundLayers);

        // If the player is grounded, set wasGrounded to false and set the jumpTimer to maxJump
        if (isGrounded)
        {
            wasGrounded = false;
            jumpTimer = maxJump;

            // If the player is grounded and the stomp multiplier is greater than 1, set the multiplier to 1
            if (stompMultiplier > 1)
            {
                stompMultiplier = 1;
            }
        }

        // Reset acceleration when either A or D key is unpressed
        if (Input.GetAxis("Horizontal") == 0)
        {
            acceleration = 0;
        }
        else
        {
            // While grounded and pressing either A or D, increase acceleration
            if (isGrounded)
            {

                acceleration += accelerationGain;

                // Clamp acceleration to the maxAcceleration variable
                if (acceleration > maxAcceleration)
                {
                    acceleration = maxAcceleration;
                }
            }
        }
    }

    // Controls player horizontal movement
    void Movement ()
    {
        // Variable for horizontal movement
        float hSpeed = Input.GetAxis("Horizontal");

        // Checks if the horizontal input is not 0 and scales the sprite accordingly
        if (hSpeed > 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (hSpeed < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        // Constantly set the rigid body's velocity X axis to the Horizontal input multiplied by the move speed
        rb.velocity = new Vector2(hSpeed * (moveSpeed + acceleration), rb.velocity.y);
    }

    // Controls player vertical movement
    void Jump ()
    {
        // If the player has been holding jump for more than 0.5 seconds in the air, then set wasGrounded to false
        if (wasGrounded && jumpTimer <= 0)
        {
            wasGrounded = false;
        }

        // If the player is grounded when they jump, set wasGrounded to true, zero their velocity and add jumpForce to velocity
        if (isGrounded)
        {
            gameManager.source.PlayOneShot(gameManager.jump);
            wasGrounded = true;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // If the player was grounded and they are holding space, decrease jumptimer by delta time and add up force
        if (wasGrounded)
        {
            jumpTimer -= Time.deltaTime;
            rb.AddForce(Vector2.up * airForce * jumpTimer, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D (Collider2D otherObject)
    {
        // If the player triggers layer 17 (enemy head) then call the enemy death function and increase the stomp multiplier
        if (otherObject.gameObject.layer == 17)
        {
            otherObject.transform.parent.gameObject.GetComponent<EnemyController>().EnemyDeath(stompMultiplier);
            stompMultiplier *= 2;
            // Zero the objects velocity and apply upwards force
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * stompForce * Time.deltaTime, ForceMode2D.Impulse);
        }

        // If the player triggers layer 13 (platform block) than call the HeadButt function from that object
        if (otherObject.gameObject.layer == 13)
        {
            otherObject.GetComponent<BlockController>().HeadButt();
            // Zero the objects velocity and apply down force
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * -stompForce * Time.deltaTime, ForceMode2D.Impulse);
        }

        // If the player triggers layer 14 (question block) then call the HeadButt function from that object
        if (otherObject.gameObject.layer == 14)
        {
            otherObject.GetComponent<QBlockController>().HeadButt();
            // Zero the objects velocity and apply down force
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * -stompForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D (Collision2D otherObject)
    {
        // If the player collides with the layer 19 (mushroom) then power up and add points
        if (otherObject.gameObject.layer == 19)
        {
            if (!isPowered)
            {
                transform.localScale += new Vector3(0, 1, 0);
                isPowered = true;
            }
            
            gameManager.source.PlayOneShot(gameManager.powerup);
            gameManager.AddPoints(gameManager.powerUpPoints);
            Destroy(otherObject.gameObject);
        }

        // If the player collides with layer 9 (enemy) then call the player death coroutine from game manager
        if (otherObject.gameObject.layer == 9)
        {
            // If the player is powered, set is powered to false
            if (isPowered == true)
            {
                transform.localScale -= new Vector3(0, 1, 0);
                isPowered = false;
            }
            // If the player is not powered, then the player dies
            else
            {
                // If the player has 1 or fewer lives remaining, call the game over coroutine
                if (gameManager.lives <= 1)
                {
                    gameManager.StartCoroutine("GameOver");
                }
                // If the player still has sufficient lives, call the player death coroutine
                else
                {
                    gameManager.StartCoroutine("PlayerDeath");
                }
            }
        }

        // If the player collides with layer 18 (void) then call the player death coroutine from game manager
        if (otherObject.gameObject.layer == 18)
        {
            // If the player has 1 or fewer lives remaining, call the game over coroutine
            if (gameManager.lives <= 1)
            {
                gameManager.StartCoroutine("GameOver");
            }
            // If the player still has sufficient lives, call the player death coroutine
            else
            {
                gameManager.StartCoroutine("PlayerDeath");
            }
        }

        // If the player collides with layer 21 (flagpole) then call the player finish coroutine from the game manager
        if (otherObject.gameObject.layer == 21)
        {
            gameManager.StartCoroutine("PlayerFinish");
        }
    }
}
