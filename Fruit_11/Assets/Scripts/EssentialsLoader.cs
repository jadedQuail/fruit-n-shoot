using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject player;
    public GameObject UIScreen;
    public GameObject gameManager;
    public GameObject hud;
    public GameObject audioManager;

    // Start is called before the first frame update
    void Start()
    {
        //Load in the player if there is not one
        if(PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }

        //Load in the UI canvas if there is not one
        if(UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

        //Load in the GameManager if there is not one
        if(GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if(HUD.instance == null)
        {
            Instantiate(hud);
        }

        if(AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
