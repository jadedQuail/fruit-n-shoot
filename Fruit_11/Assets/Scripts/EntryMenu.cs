using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static EntryMenu instance;

    public bool menuActive = false;

    // UI button text
    public Text submitText;
    public Text cancelText;

    public GameObject submitPS4;
    public GameObject cancelPS4;

    // Bools for enabling/disabling buttons on PS4
    public bool submitAllowed = true;
    public bool cancelAllowed = true;

    // Reminder text for the field being blank
    public Text reminderText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //PC vs. PS4 text
        submitPS4.SetActive(Master.instance.PS4active);
        cancelPS4.SetActive(Master.instance.PS4active);

        submitText.gameObject.SetActive(!Master.instance.PS4active);
        cancelText.gameObject.SetActive(!Master.instance.PS4active);

        // Giving the PS4 controller functionality
        if (Master.instance.PS4active && menuActive)
        {
            // Tapping into the GameOver menu's functionality because this menu doesn't have it
            if ((Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Return)) && submitAllowed && CustomSubmit.instance.field.text.Length > 0)
            {
                CustomSubmit.instance.PressSubmit();

                // Disable these buttons
                submitAllowed = false;
                cancelAllowed = false;

                // Allow use for the SubmittedMenu buttons
                SubmittedMenu.instance.tryAgainAllowed = true;
                SubmittedMenu.instance.mainMenuAllowed = true;
                SubmittedMenu.instance.quitAllowed = true;
            }
            // Case where a reminder is necessary
            if (Input.GetButtonDown("Fire2") && submitAllowed && CustomSubmit.instance.field.text.Length <= 0)
            {
                // Activate the reminder text
                reminderText.gameObject.SetActive(true);
            }

            if (Input.GetButtonDown("Circle") && cancelAllowed)
            {
                // Re-enable menu options from victory menu
                VictoryMenu.instance.tryAgainAllowed = true;
                VictoryMenu.instance.mainMenuAllowed = true;
                VictoryMenu.instance.quitAllowed = true;
                VictoryMenu.instance.recordAllowed = true;

                // Disable these buttons
                submitAllowed = false;
                cancelAllowed = false;

                // Deactivate the reminder text
                reminderText.gameObject.SetActive(false);

                // Close out
                CloseMenu();
            }
        }
    }

    //Function to open this menu
    public void OpenMenu()
    {
        //Menu opened; player can't move, menu is active, player cannot shoot
        PlayerController.instance.canMove = false;
        PlayerController.instance.canShoot = false;

        // Allow for submitting
        submitAllowed = true;

        // Disable this functionality
        VictoryMenu.instance.tryAgainAllowed = false;

        theMenu.SetActive(true);
        menuActive = true;

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }

    //Function to close this menu
    public void CloseMenu()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        // Disallow for submitting
        submitAllowed = false;

        theMenu.SetActive(false);
        menuActive = false;

        //Play UI sound
        AudioManager.instance.PlaySFX(4);
    }

    //Close the menu without a sound effect (for the GameOver menu)
    public void CloseMenuNoSound()
    {
        //Menu closed; player can move, menu is gone, player can shoot
        PlayerController.instance.canMove = true;
        PlayerController.instance.canShoot = true;

        theMenu.SetActive(false);
        menuActive = false;
    }
}
