using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        //Starting volume, on scale of 0 to 0.33608 (My scale is done this way because I haven't figured out how to make a reasonable
        //volume level fit on 0 to 1, so I'm doing this weird decimal thing (FIX THIS!)
        slider.value = 0.16804f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel ()
    {
        //Allows the slider to change the volume on the audio mixer (done with log, so volume change makes sense)
        float sliderValue = slider.value;
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}
