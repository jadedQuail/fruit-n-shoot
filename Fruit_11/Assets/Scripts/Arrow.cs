using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 15f;
    public Rigidbody2D rb;

    private Vector2 arrowVelocity;

    // Start is called before the first frame update
    void Start()
    {
        //Rotate the arrow and send it in the right direction
        gameObject.transform.rotation = Quaternion.Euler(PlayerController.instance.arrowRotation);

        arrowVelocity = PlayerController.instance.arrowDirection * speed;
        rb.velocity = arrowVelocity;
    }

    void Update()
    {
        //Pause the arrow if a menu is activated
        if (PauseMenu.instance.menuActive || GameOverMenu.instance.menuActive)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.velocity = arrowVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy the arrow if it hits a wall
        if (other.tag == "Solid Object" || other.tag == "Shield" || other.tag == "Box")
        {
            Destroy(gameObject);

            //Object destroyed; hidden arrow count drops
            GameManager.instance.hiddenArrowCount -= 1;

            //Check for game over provided that fruit is not still active
            if (!GameManager.instance.animationActive && GameManager.instance.hiddenArrowCount <= 0)
            {
                GameManager.instance.CheckForGameOver();
            }
        }

        //I have not yet fixed the issue where arrows fire in the wall if the player is over the wall's front line
        // - - - Nevermind, I fixed this by changing the Composite Collider 2D from "Outlines" to "Polygon"
    }
}
