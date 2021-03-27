using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour {

    public AudioMixer mixer;

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("volumeMaster", Mathf.Log10(sliderValue) * 20);
        mixer.SetFloat("volumeDriving", Mathf.Log10(sliderValue) * 20);
        mixer.SetFloat("volumeJungle", Mathf.Log10(sliderValue) * 20);
    }
}