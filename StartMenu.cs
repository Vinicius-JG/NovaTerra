using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] AudioClip music;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(music);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            GetComponent<Animator>().SetTrigger("Click");
    }
}
