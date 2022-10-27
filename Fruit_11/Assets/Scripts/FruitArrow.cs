using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitArrow : MonoBehaviour
{
    public Rigidbody2D rb;
    private GameObject theArrow;

    private Vector2 localVelocity;
    private bool stored = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        //Store the velocity once (this is the most disgusting thing you've ever done and you should be ashamed of yourself)
        if (stored == false)
        {
            localVelocity = rb.velocity;
            stored = true;
        }

        //Pause the arrow if a menu is activated
        if (PauseMenu.instance.menuActive || GameOverMenu.instance.menuActive || ShootBackMenu.instance.menuActive || ShieldMenu.instance.menuActive || CookieMenu.instance.menuActive)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.velocity = localVelocity;
        }
    }

    public void GetArrowInfo(Vector2 arrowVelocity, Vector3 arrowRotation, Transform firePoint, bool canShoot)
    {
        if (canShoot)
        {
            rb.velocity = arrowVelocity;

            //Instantiate
            theArrow = Instantiate(gameObject, firePoint.position, firePoint.rotation);

            //Velocity
            theArrow.GetComponent<Rigidbody2D>().velocity = arrowVelocity;

            //Rotation
            theArrow.transform.rotation = Quaternion.Euler(arrowRotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy the arrow if it hits a wall
        if (other.tag == "Solid Object" || other.tag == "Box")
        {
            Destroy(gameObject);
        }

        //Destroy the arrow if it hits a player
        if (other.tag == "Player")
        {
            Destroy(gameObject);

            //Start player's destruction animation
            PlayerController.instance.myAnim.SetBool("isHurt", true);

            if (GameManager.instance.hearts > 0)
            {
                GameManager.instance.hearts--;

                //For final score
                GameManager.instance.heartsSaved--;
            }

            //Check for game over
            GameManager.instance.CheckForGameOver();

            //Play a destructive sound
            AudioManager.instance.PlaySFX(0);

        }
    }
}

