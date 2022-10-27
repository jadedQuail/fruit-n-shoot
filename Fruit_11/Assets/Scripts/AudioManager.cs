using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE //

// I get my music from Melody Loops (melodyloops.com)

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sfx;
    public AudioSource[] bgm;

    // Start is called before the first frame update
    void Start()
    {
        //Starts up on Main Menu
        PlayBGM(1);

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(int soundToPlay)
    {
        //Play a desired sound effect
        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        //Stop all other music playing
        StopMusic();

        //Play desired music
        if (musicToPlay < bgm.Length)
        {
            bgm[musicToPlay].Play();
        }
    }

    //Function that stops all music playing
    public void StopMusic()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
