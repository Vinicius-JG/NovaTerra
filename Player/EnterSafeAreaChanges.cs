using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterSafeAreaChanges : MonoBehaviour
{
    Movement playerMovement;
    AudioClip normalSFX;
    [SerializeField] AudioClip safeAreaSFX;
    [SerializeField] AudioClip safeAreaMusic;
    [SerializeField] GameObject helmetLight;
    [SerializeField] GameObject Flashlight;
    [SerializeField] AudioClip gameplayMusic;

    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
        normalSFX = playerMovement.footstepSFX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AreaCollider")
        {
            playerMovement.footstepSFX = safeAreaSFX;
            helmetLight.SetActive(false);
            Flashlight.SetActive(false);
            AudioManager.Instance.PlayMusicWithFade(safeAreaMusic, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "AreaCollider")
        {
            playerMovement.footstepSFX = normalSFX;
            helmetLight.SetActive(true);
            Flashlight.SetActive(true);
            AudioManager.Instance.PlayMusicWithFade(gameplayMusic, true);
        }
    }
}
