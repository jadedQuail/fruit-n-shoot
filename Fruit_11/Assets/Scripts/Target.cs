using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    //  Speed:  Positive is up/right
    //          Negative is down/left

    //  Horizontal checked = left/right
    //  Vertical checked = up/down

    //  I.e. speed = -5 and horizontal is checked:
    //  object will move horizontally and to the left (until it hits a wall)                
             

    public GameObject Coin;

    public Animator myAnim;

    //Store the firing coroutine
    private Coroutine fireCoroutine;

    //Declarations for a moving target
    [Header("Movement Settings")]

    public bool isMoving;

    public bool horizontal;
    public bool vertical;

    public Rigidbody2D theRB;

    public float speed;

    [Header("Diagonal")]

    public bool diagonal;

    public float xSpeed;
    public float ySpeed;

    [Header("Key")]

    public bool isKey;

    public GameObject barrier;

    [Header("Main Menu")]

    public bool mainMenu;
    public float menuSpeed;

    [Header("Colliders")]

    public GameObject colliders;

    [Header("Shooting")]
    public bool shooting;

    public string arrowDirection;

    public float arrowSpeed;
    public float arrowInterval;
    private Vector3 arrowRotation;
    public Transform firePoint;

    public FruitArrow fruitArrow;

    private Vector2 arrowVelocity;

    //Allows the fruit to shoot
    public bool canShoot = true;

    [Header("Mine")]
    public bool isMine;
    public float mineSpeed = 2f;
    public float mineDistance = 4f;

    private Vector2 directionToPlayer;
    private float distance;

    [Header("Cookie")]
    public bool isCookie;

    // Start is called before the first frame update
    void Start()
    {
        //Force the fruit to be off the main menu if main menu is inactive (because for some reason, fruit keeps flying)
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            mainMenu = true;
            menuSpeed = 2f;
        }
        else
        {
            mainMenu = false;
            menuSpeed = 0f;
        }

        //Format the arrow if we're shooting
        if(shooting)
        {
            //Direction
            if(arrowDirection.ToLower() == "left")
            {
                arrowVelocity = new Vector2(-arrowSpeed, 0f);
                arrowRotation = new Vector3(0f, 0f, 135f);
                firePoint.localPosition = new Vector3(-0.37f, 0f, 0f);
            }
            else if(arrowDirection.ToLower() == "right")
            {
                arrowVelocity = new Vector2(arrowSpeed, 0f);
                arrowRotation = new Vector3(0f, 0f, -45f);
                firePoint.localPosition = new Vector3(0.37f, 0f, 0f);
            }
            else if (arrowDirection.ToLower() == "up")
            {
                arrowVelocity = new Vector2(0f, arrowSpeed);
                arrowRotation = new Vector3(0f, 0f, 45f);
                firePoint.localPosition = new Vector3(0f, 0.37f, 0f);
            }
            else if (arrowDirection.ToLower() == "down")
            {
                arrowVelocity = new Vector2(0f, -arrowSpeed);
                arrowRotation = new Vector3(0f, 0f, -135f);
                firePoint.localPosition = new Vector3(0f, -0.37f, 0f);
            }
        }

        //Fire an arrow every few seconds if applicable
        if (shooting)
        {
            fireCoroutine = StartCoroutine(FireArrow());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainMenu)
        {
            //Prevent movement if either menu is open
            if (PauseMenu.instance.menuActive || GameOverMenu.instance.menuActive)
            {
                //Prevent the object from moving
                isMoving = false;

                //Kill the velocity
                theRB.velocity = new Vector2(0, 0);
            }
            else
            {
                isMoving = true;
            }

            //Only relevant if this is a moving object (and no menus are open)
            if (isMoving)
            {
                //Horizontal or vertical, set speed on x and y axes (respectively)
                if (horizontal && !vertical)
                {
                    theRB.velocity = new Vector2(speed, 0);
                }

                else if (vertical && !horizontal)
                {
                    theRB.velocity = new Vector2(0, speed);
                }

                else if (diagonal)
                {
                    theRB.velocity = new Vector2(xSpeed, ySpeed);
                }
            }
        }
        else
        {
            //Settings for speed when we're on the main menu
            theRB.velocity = new Vector2(menuSpeed, 0);
        }

        //Check whether or not we can shoot based on menus
        if (GameManager.instance != null)
        {
            if (GameManager.instance.MenuOpen() == true)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;

                //Restart Coroutine ONCE
            }
        }

        if(isMine)
        {
            if (PlayerController.instance != null)
            {
                //Lemon will follow the player
                directionToPlayer = PlayerController.instance.transform.position - transform.position;

                //Get the distance
                distance = Vector2.Distance(transform.position, PlayerController.instance.transform.position);

                if (!GameOverMenu.instance.menuActive && !PauseMenu.instance.menuActive && !LemonMenu.instance.menuActive && distance < mineDistance)
                {
                    theRB.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y).normalized * mineSpeed;
                }
                else
                {
                    theRB.velocity = new Vector2(0, 0);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If an arrow has hit this target, then the target should start flashing
        if(other.tag == "Arrow" && isCookie == false)
        {
            //Destroy the arrow
            Destroy(other.gameObject);

            //If the object is moving, force it to stop
            speed = 0f;
            xSpeed = 0f;
            ySpeed = 0f;
            mineSpeed = 0f;

            //Play a destruction sound
            AudioManager.instance.PlaySFX(3);

            //Destroy the colliders to prevent another arrow from hitting the fruit in the process of destruction
            Destroy(colliders);

            //Fruit can no longer shoot once destruction has begun
            canShoot = false;

            //Immediately stops the shooting coroutine
            if (shooting)                               //This if statement is necessary, fruits refuse to respond if you try and
            {                                           //stop a coroutine that doesn't exist
                StopCoroutine(fireCoroutine);
            }

            //Starts the animation, signals to the function below that the item is ready to be destroyed
            myAnim.SetBool("isDestroyed", true);

            //Indicates an animation is occurring
            GameManager.instance.animationActive = true;
        }

        //Logic for cookies
        if(other.tag == "Arrow" && isCookie == true)
        {
            //Destroy the arrow
            Destroy(other.gameObject);
        }

        if (other.tag == "Solid Object")
        {
            //If the object hits a wall or another fruit, reverse the direction of the object (one-axis only)
            speed = -speed;
        }

        if (other.tag == "TopBot")
        {
            //Only reverse the ySpeed (top or bottom bumper hit)
            ySpeed = -ySpeed;
        }

        if(other.tag == "LeftRight")
        {
            //Only reverse the xSpeed (left or right bumper hit)
            xSpeed = -xSpeed;
        }

        if(other.tag == "Despawner")
        {
            //Despawn the fruit
            Destroy(gameObject);
        }

        if(other.tag == "Player" && isMine)
        {
            //Stop the lemon from moving
            mineSpeed = 0;

            //Play a destructive sound
            AudioManager.instance.PlaySFX(0);

            //Take away player hearts (never goes below 0)
            if(GameManager.instance.hearts == 1)
            {
                GameManager.instance.hearts -= 1;
                GameManager.instance.heartsSaved -= 1;
            }
            else if(GameManager.instance.hearts > 0)
            {
                GameManager.instance.hearts -= 2;
                GameManager.instance.heartsSaved -= 2;
            }

            //Check for game over
            GameManager.instance.CheckForGameOver();

            //Account for target eliminated
            GameManager.instance.currentSceneTargets--;

            //Begin the explosion animation
            myAnim.SetBool("isExploding", true);
        }
    }

    public void ExplosionDone(string message)
    {
        //Explosion is over
        if (message.Equals("ExplosionAnimationEnded"))
        {
            //Destroy the game object
            Destroy(gameObject);
        }
    }

    public void AlertObservers(string message)
    {
        //The item has been shot and once the animation ends, it will send this message
        if (message.Equals("DestroyAnimationEnded"))
        {
            //Destroy the object
            Destroy(gameObject);

            //Animation is over
            GameManager.instance.animationActive = false;

            //Remove hidden arrow
            GameManager.instance.hiddenArrowCount -= 1;

            //Instantiate a coin in the object's place (provided it's a fruit and not a key)
            if (!isKey)
            {
                //Remove the target from the requirement to exit the scene (fruit only)
                GameManager.instance.currentSceneTargets -= 1;

                Instantiate(Coin).transform.position = gameObject.transform.position;
            }

            if (isKey)
            {
                //Call the function that will set the barrier's animation and destruction in motion
                barrier.GetComponent<Barrier>().DestroyBarrier();
            }

            //Check for game over (both keys and fruit)
            if (GameManager.instance.hiddenArrowCount <= 0)
            {
                GameManager.instance.CheckForGameOver();
            }

        }
    }

    public IEnumerator FireArrow()
    {
        //Continuously fire arrows every 3 seconds
        while (true)
        {
            if (canShoot)
            {
                //Arrow can shoot (i.e. we're not paused or finished)
                yield return new WaitForSeconds(arrowInterval/2);
                fruitArrow.GetArrowInfo(arrowVelocity, arrowRotation, firePoint, canShoot);
                yield return new WaitForSeconds(arrowInterval/2);
            }
            else
            {
                //Arrow cannot shoot (paused or finished)   //We do 0.5 seconds so as to give a fair amount of time before shooting begins again
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
