using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Coin is collected if the player runs over it.
        if(other.tag == "Player")
        {
            //Destroy the coin
            Destroy(gameObject);

            //Play a pickup sound effect
            AudioManager.instance.PlaySFX(6);

            //Add to the player's coins
            GameManager.instance.coinCount += 1;

            //Display the coins
            HUD.instance.UpdateCoins();
        }
    }

}
