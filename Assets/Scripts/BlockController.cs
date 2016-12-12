using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    Animator anim;
    public GameManager gameManager;
    public LayerMask layer;
    public int rayDist = 20;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}

    // Coroutine that allows mecanim animation to play only once
    public IEnumerator PlayOneShot (string paraName)
    {
        anim.SetBool(paraName, true);
        yield return null;
        anim.SetBool(paraName, false);
    }

    // Function that controls when the player hits their head on a block
    public virtual void HeadButt ()
    {
        StartCoroutine(PlayOneShot("headButt"));

        // Cast a ray up to see if there are any objects above the block (filter only layers from the layer LayerMask)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayDist, layer);

        // If the ray hits layer 9 (enemy), call the enemy death function from the enemy's script
        if (hit && hit.collider.gameObject.layer == 9)
        {
            hit.collider.gameObject.GetComponent<EnemyController>().EnemyDeath(1);
        }

        // If the ray hits layer 19 (mushroom), call the swap direction function from the mushroom's script
        else if (hit && hit.collider.gameObject.layer == 19)
        {
            hit.collider.gameObject.GetComponent<MushroomMovement>().SwapDirection(hit.collider.gameObject.GetComponent<MushroomMovement>().heading);
        }
    }
}
