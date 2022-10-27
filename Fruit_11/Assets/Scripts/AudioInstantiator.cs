using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstantiator : MonoBehaviour
{
    public GameObject audioManager;

    //Only meant to be called on the Main Menu!

    // Start is called before the first frame update
    void Start()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
