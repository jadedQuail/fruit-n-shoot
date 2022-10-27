using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    public Image fadeScreen;

    public float fadeSpeed;

    private bool shouldFadeToBlack;
    private bool shouldFadeFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //If we should be fading to black, then we will; once we reach our destination, stop fading to black.
        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 
                                         Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        //If we should be fading from black, then we will; once we reach our destination, stop fading from black.
        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                                         Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;

                //Makes it so the player can move again after the fade
                PlayerController.instance.canMove = true;
            }
        }
    }

    //Function that allows other scripts to call a fade to black
    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;

        //Makes it so the player cannot move during the fade
        PlayerController.instance.canMove = false;

        //Makes it so the player cannot shut during the fade
        PlayerController.instance.canShoot = false;
    }

    //Function that allows other scripts to call a fade from black
    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;

        //Makes it so the player can move again after the fade
        PlayerController.instance.canMove = true;

        //Makes it so the player can shoot again after the fade
        PlayerController.instance.canShoot = true;
    }
}
