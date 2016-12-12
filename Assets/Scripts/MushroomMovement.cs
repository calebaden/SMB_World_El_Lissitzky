using UnityEngine;
using System.Collections;

public class MushroomMovement : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private bool isRising = true;
    private Vector2 startingPos;

    public int heading = 0;
    public Vector2 direction = Vector2.right;

	// Use this for initialization
	void Start ()
    {
        // Sets the starting position vector to the mushroom position on instantiate
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // If the mushroom is not rising, move it torwards the direction Vector at move speed
        if (!isRising)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        // If the muchroom is rising, move it in the up Vector at move speed
        else
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }

        // If the mushroom is rising and it reaches 1 unit above its starting position, 
        // set isRising to false, set its collider to non trigger, and set the rigid body's gravity scale to 1
        if (isRising && transform.position.y - startingPos.y > 1)
        {
            isRising = false;
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
	}

    // Function that checks what direction the mushroom is traveling and sets the direction vector accordingly
    public void SwapDirection (int _heading)
    {
        if (_heading == 0)
        {
            direction = Vector2.left;
            heading = 1;
        }
        else if (_heading == 1)
        {
            direction = Vector2.right;
            heading = 0;
        }
    }

    void OnCollisionEnter2D (Collision2D otherObject)
    {
        // When the mushroom collides with a wall, change it's direction
        if (otherObject.collider.gameObject.layer == 16)
        {
            SwapDirection(heading);
        } 

        // When the mushroom collides with the void, destroy it
        if (otherObject.collider.gameObject.layer == 18)
        {
            Destroy(gameObject);
        }
    }
}
