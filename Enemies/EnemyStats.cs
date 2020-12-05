using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float life;
    public bool invulnerable;
    Animator anim;

    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip protect;
    [SerializeField] AudioClip preboss;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void GetVulnerable()
    {
        invulnerable = false;
    }

    public void GetInvulnerable()
    {
        invulnerable = true;
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable)
        {
            AudioManager.Instance.PlaySFX(protect);
            return;
        }


        life -= damage;
        AudioManager.Instance.PlaySFX(hit);

        anim.SetTrigger("Damaged");

        if (life <= 0)
        {
            life = 0;
            Die();
        }
    }

    void Die()
    {
        if (gameObject.name == "Atlas")
        {
            GameObject.FindObjectOfType<DialogueManager>().startBossFight = false;
            anim.SetTrigger("Die");
            AudioManager.Instance.PlayMusicWithFade(preboss, true);
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else
        {
            GameObject.FindObjectOfType<GameController>().FirstBloodDialogue();
            Destroy(gameObject);
        }

    }
}
