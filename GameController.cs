using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip music2;
    [Range(0.0f, 1.0f)]
    public float musicVolume = 0.5f;
    [Range(0.0f, 1.0f)]
    public float sfxVolume = 0.5f;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] GameObject pauseMenu;
    public bool paused;
    public bool inDialogue;

    bool firstBlood;
    bool finishGame;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(music);
    }

    void Update()
    {

        if (GameObject.Find("Atlas") == null && !finishGame)
        {
            finishGame = true;
            GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("EndGame");
            StartCoroutine(EndGame());
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                paused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                paused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

        AudioManager.Instance.SetSFXVolume(sfxVolume);
        AudioManager.Instance.SetMusicVolume(musicVolume);
    }

    public void ChangeVolume()
    {
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;
    }

    public void DialogueStarted()
    {
        inDialogue = true;
        GameObject.FindObjectOfType<Player>().DialogueStarted();
    }

    public void DialogueEnded()
    {
        inDialogue = false;
    }

    public void FirstBloodDialogue()
    {
        if (firstBlood)
            return;

        GetComponent<DialogueTrigger>().TriggerDialogue();
        firstBlood = true;
    }

    public void StartBossFight()
    {
        AudioManager.Instance.PlayMusic(music2);
        GameObject.FindObjectOfType<Atlas>().start = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
