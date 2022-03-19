using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider volSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void Start()
    {
        volSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1f);

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volSlider.value) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);

    }

    public void SetTotalVolume(float volume)
    {
        //volume = volSlider.value;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        //PlayerPrefs.SetFloat("MasterVolume", volume);

        //PlayerPrefs.Save();
    }

    public void SetMusic(float volume)
    {
        PlayerPrefs.SetFloat("Music", volume);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFX", volume);
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
