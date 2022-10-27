using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;

    [Header("Entrance/Exit Name")]
    public string areaEntranceName;
    public string areaExitName;

    [Header("Entering/Exiting")]
    public bool entering;
    public bool exiting;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    ///////////////////////////////////////////////////////////////////////////////////
    ///     If you're having problems with the scene not fading from black, it's    ///
    ///     probably because a portal was configured wrong (entering/exiting flag   ///
    ///     or name)                                                                ///
    ///////////////////////////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //Allows the player to spawn in if it's the right portal and he's meant to enter
        if (areaEntranceName == PlayerController.instance.areaTransitionName && entering)
        {
            PlayerController.instance.transform.position = transform.position;

            //A player is entering a scene; has to fade from black!
            UIFade.instance.FadeFromBlack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //When this is true, the countdown begins; when the countdown is finished, we load the next scene
        //(We do a countdown so as to allow time for fading to black)
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;

            if (waitToLoad <= 0)
            {
                //Load the new scene
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);

                //Check for targets in the new scene
                GameManager.instance.CheckForTargets(sceneToLoad);

                //Make sure the player is facing downward in the new scene
                PlayerController.instance.myAnim.SetFloat("lastMoveY", -1);

                //Make sure the arrow is going downward in the new scene
                PlayerController.instance.SetArrow("Down");

                //Set the arrow count
                HUD.instance.UpdateArrows();

                //Give the player health if we've reached that part of the game (Floor 6)
                sceneToLoad = sceneToLoad.Replace("Floor", "");

                if (int.Parse(sceneToLoad) >= 6 && int.Parse(sceneToLoad) < 11)
                {
                    GameManager.instance.hearts = 3;
                }
                else if (int.Parse(sceneToLoad) >= 11)
                {
                    GameManager.instance.hearts = 5;
                }
            }
        }
    }

    //Player enters portal and spawns in a new scene
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && exiting)
        {
            //Set the bool so that the countdown will begin, and fade to black
            shouldLoadAfterFade = true;
            UIFade.instance.FadeToBlack();

            //Gives to the player the name of the portal being used to exit
            PlayerController.instance.areaTransitionName = areaExitName;
        }
    }
}
