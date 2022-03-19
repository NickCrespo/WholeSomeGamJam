using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetTotalVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSFX(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
}