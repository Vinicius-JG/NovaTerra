using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool bossFightArea;
    [SerializeField] AudioClip preBoss;
    public GameObject[] destroyOnDialogueStart;


    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if (bossFightArea)
        {
            FindObjectOfType<DialogueManager>().startBossFight = true;
            GameObject.Find("BossCam").GetComponent<CinemachineVirtualCamera>().Priority = 100;
            GameObject.FindObjectOfType<Movement>().FlipLeft();
            AudioManager.Instance.PlayMusicWithFade(preBoss, true);
        }
        for (int i = 0; i < destroyOnDialogueStart.Length; i++)
        {
            Destroy(destroyOnDialogueStart[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameObject.tag != "Enemy")
            TriggerDialogue();
    }
}
