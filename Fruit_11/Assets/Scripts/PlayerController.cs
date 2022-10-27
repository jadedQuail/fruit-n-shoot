using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the movement of the player

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    public Animator myAnim;

    public static PlayerController instance;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    //Allows the player to move
    public bool canMove;

    //Allows the player to shoot
    public bool canShoot;

    public bool brandNewGame;

    //Holds the name of the portal that the player is using
    public string areaTransitionName;

    //Point from which the bullet fires, its direction, its rotation
    public Transform firePoint;
    public Vector3 arrowDirection;
    public Vector3 arrowRotation;

    // Speed for movement
    public float xSpeed;
    public float ySpeed;

    // Number for sensitivity of character animation
    public float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);

        //C# is ignorant and refuses to allow me to initially set "canMove" as true, for whatever reason. So I'm doing it here.
        canMove = true;
        canShoot = true;

        //When this first opens up, we have a brand new game; this flag will prevent the timer from running until movement
        brandNewGame = true;

        //Make sure the player is facing downward in the initial scene
        myAnim.SetFloat("lastMoveY", -1);

        //Make sure the arrow is going downward in the initial scene
        SetArrow("Down");
    }

    // Update is called once per frame
    void Update()
    {
        //Player moves via Rigidbody2D's velocity, takes input from Unity's inherent axes
        if (canMove)
        {
            // Diagonal movement
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
            {
                //Movement occurs, start the timer and end the "new game" period
                GameOverMenu.instance.newGame = false;
                brandNewGame = false;

                //This accounts for extra speed from moving along the hypotenuse (it gets eliminated by this code)
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * (1 / new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude);
            }
            // Non-diagonal movement
            else
            {
                //Movement occurs, start the timer and end the "new game" period (check if movement is actually occurring)
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    GameOverMenu.instance.newGame = false;
                    brandNewGame = false;
                }

                // Speed x
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    xSpeed = 1f;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    xSpeed = -1f;
                }

                // Speed y
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    ySpeed = 1f;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    ySpeed = -1f;
                }

                //No speed
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    xSpeed = 0;
                }

                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    ySpeed = 0;
                }

                //Movement that is not faster
                //theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
                theRB.velocity = new Vector2(xSpeed, ySpeed) * moveSpeed;
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        //Sets the floats from the animators, so the animator knows which direction the sprite needs to face
        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        // Multiply the raw input so as to have more values to work with when dealing with sensitivity
        float xSens = Input.GetAxisRaw("Horizontal") * 100;
        float ySens = Input.GetAxisRaw("Vertical") * 100;

        //Sets things so that we're idling in the right direction
        if (xSens > sensitivity || xSens < -sensitivity || ySens > sensitivity || ySens < -sensitivity)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", theRB.velocity.x);
                myAnim.SetFloat("lastMoveY", theRB.velocity.y);
            }
        }

        /////////////////////////////
        ///         ARROW         ///
        /////////////////////////////

        //Find out what direction the character is facing and assign the shooting point (when standing still and moving)
        //Also assign the direction and rotation of the arrow that will be fired

        if(xSens > sensitivity)        //Right
        {
            SetArrow("Right");
        }

        if (xSens < -sensitivity)   //Left
        {
            SetArrow("Left");
        }

        if (ySens > sensitivity)   //Up
        {
            SetArrow("Up");
        }

        if (ySens < -sensitivity)   //Down
        {
            SetArrow("Down");
        }


        //(This is probably unneccessary, but I'm going to add it anyway)
        //Clamps the player within the bounds of the map
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
                                         Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
                                         transform.position.z);
    }

    //Sets the bounds for the player (convenient to put it here, called by the CameraController script)
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(0.5f, 1f, 0f);
        topRightLimit = topRight - new Vector3(0.5f, 1f, 0f);
        //The new vectors add some padding to the edge of the map (so the player isn't hanging off the side)
    }

    //Makes sure the firepoint, velocity, and rotation are correct for a given direction
    public void SetArrow(string direction)
    {
        if(direction == "Up")
        {
            firePoint.localPosition = new Vector3(0, 0.75f, 0);
            arrowDirection = new Vector3(0f, 1f, 0f);
            arrowRotation = new Vector3(0f, 0f, 45f);
        }
        else if(direction == "Down")
        {
            firePoint.localPosition = new Vector3(0, -0.75f, 0);
            arrowDirection = new Vector3(0f, -1f, 0f);
            arrowRotation = new Vector3(0f, 0f, -135f);
        }
        else if(direction == "Left")
        {
            firePoint.localPosition = new Vector3(-0.4f, -0.1f, 0);
            arrowDirection = new Vector3(-1f, 0f, 0f);
            arrowRotation = new Vector3(0f, 0f, 135f);
        }
        else if(direction == "Right")
        {
            firePoint.localPosition = new Vector3(0.4f, -0.1f, 0);
            arrowDirection = new Vector3(1f, 0f, 0f);
            arrowRotation = new Vector3(0f, 0f, -45f);
        }
        else
        {
            Debug.Log("Invalid direction for arrow!");
        }
    }

    //End the player's suffering (like, he's okay now, not dead)
    public void AlertObservers(string message)
    {
        if(message.Equals("HurtAnimationEnded"))
        {
            myAnim.SetBool("isHurt", false);
        }
    }
}
