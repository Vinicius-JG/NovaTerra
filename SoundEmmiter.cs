using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmmiter : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip sfx;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound()
    {
        audioSource.PlayOneShot(sfx);
    }
}
