using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumeSound : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0.16804f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Function that changes the SFX volume when the slider is moved
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);
    }
}
