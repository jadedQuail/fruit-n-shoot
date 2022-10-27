using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Quick note: SHIFT + TAB to reverse indent!

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public Text coinText;
    public Text arrowText;
    public Text timerText;

    public float currentTime;

    private string minutes;
    private string seconds;
    private string miliseconds;

    //Counting minutes and seconds for the final score
    public float finalTime;

    public Animator myAnim;

    //Hearts (health)
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    // Start is called before the first frame update
    void Start()
    {
        //Instance the HUD
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Wrapper that makes sure the timer stops when you're paused for any reason
        if (GameManager.instance.MenuOpen() == false)
        {
            //Update the time (but not if we're in the new game timeframe)
            if (!GameOverMenu.instance.newGame && !PlayerController.instance.brandNewGame)
            {
                currentTime += Time.deltaTime;
                finalTime = currentTime;
            }
            //Format minutes
            if ((int)currentTime / 60 < 10)
            {
                minutes = "0" + ((int)currentTime / 60).ToString();
            }
            else
            {
                minutes = ((int)currentTime / 60).ToString();
            }

            //Format seconds
            if (currentTime % 60 < 10)
            {
                seconds = "0" + (currentTime % 60).ToString("f2");
            }
            else
            {
                seconds = (currentTime % 60).ToString("f2");
            }

            //Update the timer text
            timerText.text = minutes + ":" + seconds;
        }

        //Show Hearts
        ShowHearts();
    }

    public void UpdateCoins()
    {
        //Update the coins on screen (to be called from elsewhere)
        if(GameManager.instance.coinCount < 10)
        {
            //Add a zero in front if it's not double digits
            coinText.text = "0" + GameManager.instance.coinCount.ToString();
        }
        else
        {
            //No zero necessary
            coinText.text = GameManager.instance.coinCount.ToString();
        }
    }

    public void UpdateArrows()
    {
        //Update the arrows on screen (to be called from elsewhere)
        if(GameManager.instance.arrowCount < 10)
        {
            //Add a zero in front if it's not double digits
            arrowText.text = "0" + GameManager.instance.arrowCount.ToString();
        }
        else
        {
            //No zero neccessary
            arrowText.text = GameManager.instance.arrowCount.ToString();
        }
    }

    //Function that signals the game to stop moving the menu down onto the screen (Game Over Menu)
    public void GameOverMovementDone(string message)
    {
        if(message == "Finished")
        {
            myAnim.SetBool("moveToOpen", true);
        }
    }

    //Function that signals the game to stop moving the menu down onto the screen (Victroy Menu)
    public void VictoryMovementDone(string message)
    {
        if(message == "vFinished")
        {
            myAnim.SetBool("v_moveToOpen", true);
        }
    }

    public void ShowHearts()
    {
        //Dead or disabled; no hearts show
        if (GameManager.instance.hearts <= 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
            heart5.SetActive(false);
        }
        else if (GameManager.instance.hearts == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
            heart4.SetActive(false);
            heart5.SetActive(false);
        }
        else if (GameManager.instance.hearts == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
            heart4.SetActive(false);
            heart5.SetActive(false);
        }
        else if (GameManager.instance.hearts == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(false);
            heart5.SetActive(false);
        }
        else if (GameManager.instance.hearts == 4)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
            heart5.SetActive(false);
        }
        else if (GameManager.instance.hearts == 5)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            heart4.SetActive(true);
            heart5.SetActive(true);
        }

    }
}
