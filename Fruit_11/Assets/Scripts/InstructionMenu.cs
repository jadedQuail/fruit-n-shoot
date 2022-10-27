using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static InstructionMenu instance;

    public bool menuActive = false;

    //UI Components
    public Text explanationText;

    // Different UI for controller vs. kbm
    public Text actionButtonTextPC;
    public GameObject actionButtonTextPS4;
    public Text actionButtonTextPS4_words;

    public Image explanationImage;

    public string[] instructionTexts;
    public Sprite[] instructionSprites;

    public int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if PS4 is active, adjust UI
        actionButtonTextPC.gameObject.SetActive(!Master.instance.PS4active);
        actionButtonTextPS4.SetActive(Master.instance.PS4active);

        //Allow the enter key to hit "Continue"
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire2")) && menuActive == true)
        {
            Continue();
        }
    }

    //Close the menu
    public void CloseMenu()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        theMenu.SetActive(false);
        menuActive = false;

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }

    //Open the menu
    public void OpenMenu()
    {
        //Menu opened; player can't move, menu is active, player cannot shoot
        PlayerController.instance.canMove = false;
        PlayerController.instance.canShoot = false;

        theMenu.SetActive(true);
        menuActive = true;
    }

    public void Continue()
    {
        //Advance the counter to bring up the next items
        counter = counter + 1;

        //Change the text and image
        if (counter == 3)
        {
            explanationText.text = instructionTexts[counter];
            explanationImage.sprite = instructionSprites[counter];
            actionButtonTextPC.text = "Got It";
            actionButtonTextPS4_words.text = "Got It";


        }
        else if (counter == 4)
        {
            CloseMenu();
        }
        else
        {
            explanationText.text = instructionTexts[counter];
            explanationImage.sprite = instructionSprites[counter];
        }

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }
}
