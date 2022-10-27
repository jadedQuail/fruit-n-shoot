using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] targets;
    public int currentSceneTargets;

    public int[] keys;
    public int currentSceneKeys;

    private int sceneNumber;

    //The number of coins the player has collected
    public int coinCount = 0;

    //The number of arrows the player has left

    public int arrowCount; //Arrow is gone after the player fires
    public int hiddenArrowCount; //Arrow is gone after it is destroyed

    //Game ends on the hiddenArrowCount ("Game isn't over until the last arrow is dead")

    //Bools for checking to see if a destruction animation is occurring
    public bool animationActive = false;

    public GameObject Arrow;

    private bool instrLoad;
    private bool shootBackLoad;
    private bool shieldLoad;
    private bool lemonLoad;
    private bool cookieLoad;
    private bool keyLoad;

    public bool gameEnded;

    public bool victoryMenuActive;

    //Player health
    public int hearts;

    //Hearts saved, factored into final score
    public int heartsSaved = 30;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        //instrLoad = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Keep the arrows updated
        if (HUD.instance != null)
        {
            HUD.instance.UpdateArrows();
        }

        //If we have a newGame, the scene is 0
        if (GameOverMenu.instance != null)
        {
            if (GameOverMenu.instance.newGame)
            {
                sceneNumber = 0;
            }
        }

        //Load the instruction pane once, and only on the first load
        if (instrLoad == false && Master.instance.reachedInstructions == false)
        {
            if (InstructionMenu.instance != null)
            {
                InstructionMenu.instance.OpenMenu();
            }
            instrLoad = true;
            Master.instance.reachedInstructions = true;
        }

        //Load the shoot back menu once, and only on the first load
        if (shootBackLoad == false && Master.instance.reachedReturnFire == false)
        {
            if (sceneNumber == 6)
            {
                ShootBackMenu.instance.OpenMenu();
                shootBackLoad = true;
                Master.instance.reachedReturnFire = true;
            }
        }

        //Load the shield menu once, and only on the first load
        if (shieldLoad == false && Master.instance.reachedShield == false)
        {
            if (sceneNumber == 7)
            {
                ShieldMenu.instance.OpenMenu();
                shieldLoad = true;
                Master.instance.reachedShield = true;
            }
        }

        //Load the lemon menu once, and only on the first load
        if (lemonLoad == false && Master.instance.reachedLemon == false)
        {
            if (sceneNumber == 9)
            {
                LemonMenu.instance.OpenMenu();
                lemonLoad = true;
                Master.instance.reachedLemon = true;
            }
        }

        //Load the key menu once, and only on the first load
        if (keyLoad == false && Master.instance.reachedKey == false)
        {
            if (sceneNumber == 5)
            {
                KeyMenu.instance.OpenMenu();
                keyLoad = true;
                Master.instance.reachedKey = true;
            }
        }

        //Load the cookie menu once, and only on the first load
        if (cookieLoad == false && Master.instance.reachedCookie == false)
        {
            if (sceneNumber == 11)
            {
                CookieMenu.instance.OpenMenu();
                cookieLoad = true;
                Master.instance.reachedCookie = true;
            }
        }

        if (sceneNumber == 14)
        {
            if (gameEnded == false) 
            {
                //End the game
                EndGame();
                gameEnded = true;
            }

            //Prevent the player from moving or shooting
            PlayerController.instance.canMove = false;
            PlayerController.instance.canShoot = false;
        }
    }

    //This function checks how many targets are in this scene
    public void CheckForTargets(string sceneName)
    {
        //Get a number for the current scene
        sceneName = sceneName.Replace("Floor", "");
        sceneNumber = int.Parse(sceneName);

        //Searches array, looking for current scene
        for(int i = 0; i < targets.Length; i++)
        {
            if(i == sceneNumber)
            {
                //Set targets, set keys
                currentSceneTargets = targets[i];
                currentSceneKeys = keys[i];

                //Arrow count is targets + keys + 5
                arrowCount = currentSceneTargets + currentSceneKeys + 5;
                //Hidden arrow count follows suit
                hiddenArrowCount = arrowCount;
            }
        }
    }

    public void CheckForGameOver()
    {
        if ((arrowCount <= 0 && currentSceneTargets > 0) || (hearts <= 0 && sceneNumber >= 6 && currentSceneTargets > 0))
        {
            //Activate the menu
            GameOverMenu.instance.OpenMenu();

            //Select a quote
            GameOverMenu.instance.SelectQuote();

            //this will stop the timer
            GameOverMenu.instance.menuActive = true;

            //Force the pause menu to go away
            PauseMenu.instance.CloseMenuNoSound();

            //Play a death sound effect and then Game Over music
            AudioManager.instance.PlaySFX(0);
            AudioManager.instance.PlayBGM(0);

            //Animate the menu coming back in
            HUD.instance.myAnim.SetBool("isOpen", true);

            HUD.instance.myAnim.SetBool("moveToClose", false);

            //Player can't move or shoot
            PlayerController.instance.canMove = false;
            PlayerController.instance.canShoot = false;
        }
    }

    public void EndGame()
    {
        //Play the victory music
        AudioManager.instance.PlayBGM(2);

        //Start the animation
        HUD.instance.myAnim.SetBool("v_isOpen", true);
        HUD.instance.myAnim.SetBool("v_moveToClose", false);

        //Force the pause menu to go away (on the off chance it is open)
        PauseMenu.instance.CloseMenuNoSound();

        // Indicate that the victory menu is open (and stop the timer)
        VictoryMenu.instance.menuActive = true;

        //Prevent the player from moving or shooting
        PlayerController.instance.canMove = false;
        PlayerController.instance.canShoot = false;
    }

    //Checks if there are any menus open
    public bool MenuOpen()
    {
        if (GameOverMenu.instance != null)
        {
            if (PauseMenu.instance.menuActive || GameOverMenu.instance.menuActive
    || InstructionMenu.instance.menuActive || ShootBackMenu.instance.menuActive
    || ShieldMenu.instance.menuActive || LemonMenu.instance.menuActive
    || CookieMenu.instance.menuActive || KeyMenu.instance.menuActive
    || EntryMenu.instance.menuActive || SubmittedMenu.instance.menuActive
    || VictoryMenu.instance.menuActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            if (PauseMenu.instance.menuActive
|| InstructionMenu.instance.menuActive || ShootBackMenu.instance.menuActive
|| ShieldMenu.instance.menuActive || LemonMenu.instance.menuActive
|| CookieMenu.instance.menuActive || KeyMenu.instance.menuActive
|| EntryMenu.instance.menuActive || SubmittedMenu.instance.menuActive
|| VictoryMenu.instance.menuActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
