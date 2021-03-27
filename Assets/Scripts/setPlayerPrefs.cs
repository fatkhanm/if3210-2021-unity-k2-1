using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class setPlayerPrefs : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public InputField Username_textBox;

 
    public void AdjustVolume(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
        audioMixer.SetFloat("volumeMaster", Mathf.Log10(volume) * 20);
    }

    public void SetUsername(string name)
    {
        PlayerPrefs.SetString("Name", name);
        Debug.Log(Username_textBox.text);
        Debug.Log(name);
    }
}
