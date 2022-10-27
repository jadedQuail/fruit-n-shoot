using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyMenu : MonoBehaviour
{
    public GameObject theMenu;
    public static KeyMenu instance;

    public bool menuActive = false;

    public Text gotIt;
    public GameObject gotItPS4;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //PS4 vs. PC for UI text
        gotItPS4.SetActive(Master.instance.PS4active);
        gotIt.gameObject.SetActive(!Master.instance.PS4active);

        //Prevent the player from moving when the menu is active (to avoid conflict with FadeFromBlack)
        if (menuActive)
        {
            PlayerController.instance.canMove = false;
        }

        //Allow the enter key to close this menu
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire2")) && menuActive == true)
        {
            CloseMenu();
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
}

