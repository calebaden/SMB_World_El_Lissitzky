using UnityEngine;
using System.Collections;

public class QBlockController : BlockController
{
    public int blockType;
    public GameObject coin;
    public GameObject mushroom;
    private bool isActive = true;
    private int pointWorth = 200;
    private float lifeTime = 0.5f;

    // Function that controls when the player hits their head on a block
    public override void HeadButt()
    {
        if (isActive)
        {
            StartCoroutine(PlayOneShot("headButt"));
            isActive = false;
            transform.FindChild("QBlockAnim").gameObject.SetActive(false);

            // If the block is type 0 then it will contain a coin
            if (blockType == 0)
            {
                gameManager.source.PlayOneShot(gameManager.coin);
                // Add points to game manager and increment coins
                gameManager.AddPoints(pointWorth);
                gameManager.coins++;
                // Instantiate a new coin as a game object, then destroy it after its lifetime expires
                GameObject newCoin = Instantiate(coin, transform) as GameObject;
                Destroy(newCoin.gameObject, lifeTime);
            }
            // If the block is type 1, then it will contain a mushroom
            else
            {
                Instantiate(mushroom, transform.position, transform.rotation);
            }
        }
    }
}
