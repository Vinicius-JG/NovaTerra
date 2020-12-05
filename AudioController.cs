using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float musicVolume;
    [Range(0.0f, 1.0f)]
    public float sfxVolume;
 
    // Update is called once per frame
    void Update()
    {
        AudioManager.Instance.SetMusicVolume(musicVolume);
        AudioManager.Instance.SetSFXVolume(sfxVolume);
    }
}
