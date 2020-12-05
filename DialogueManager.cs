using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public Image speakerNameIMG;
    public Image frameIMG;
    public Image faceIMG;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<Dialogue.DialogueStep> dialogueSteps;
    public bool startBossFight;

    [SerializeField] AudioClip typeSFX;
    [SerializeField] Sprite icarusPortrait;
    [SerializeField] Sprite daedalusPortrait;
    [SerializeField] Sprite atlasPortrait;

    [SerializeField] AudioClip click;
    bool firstDialogue = true;

    bool canSkip;

    // Start is called before the first frame update
    void Start()
    {
        dialogueSteps = new Queue<Dialogue.DialogueStep>();
    }

    private void Update()
    {
        if (animator.GetBool("IsOpen"))
        {
            if (Input.GetMouseButtonDown(0) && canSkip)
            {
                AudioManager.Instance.PlaySFX(click);
                DisplayNextSentence();
            }
        }
    }

    IEnumerator SkipDelay()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.8f);
        canSkip = true;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        StartCoroutine(SkipDelay());

        dialogueSteps.Clear();

        foreach (Dialogue.DialogueStep dialogueStep in dialogue.dialogueSteps)
        {
            dialogueSteps.Enqueue(dialogueStep);
        }

        DisplayNextSentence();

        GameObject.FindObjectOfType<GameController>().DialogueStarted();
    }

    public void DisplayNextSentence()
    {
        if (dialogueSteps.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue.DialogueStep dialogueStep = dialogueSteps.Dequeue();

        dialogueText.text = dialogueStep.sentence;
        nameTxt.text = dialogueStep.nameTxt.ToUpper();
        if (dialogueStep.nameTxt == "Atlas")
            nameTxt.color = Color.red;
        else
            nameTxt.color = new Color32(37, 199, 201, 255);
        speakerNameIMG.sprite = dialogueStep.speakerName;

        if (dialogueStep.nameTxt == "Icarus")
        {
            frameIMG.sprite = icarusPortrait;
        }
        if (dialogueStep.nameTxt == "Daedalus")
        {
            frameIMG.sprite = daedalusPortrait;
        }
        if (dialogueStep.nameTxt == "Atlas")
        {
            frameIMG.sprite = atlasPortrait;
        }
    }

    void EndDialogue()
    {
        if (GameObject.Find("Atlas").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dying"))
            Destroy(GameObject.Find("Atlas"));

        animator.SetBool("IsOpen", false);
        GameObject.FindObjectOfType<GameController>().DialogueEnded();
        if (startBossFight)
            GameObject.FindObjectOfType<GameController>().StartBossFight();

        if (firstDialogue)
        {
            GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("ShowObjective");
            firstDialogue = false;
        }
    }
}
