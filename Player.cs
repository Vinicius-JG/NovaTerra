using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public float maxHealth = 100;
    [HideInInspector] public float maxShield = 100;
    [HideInInspector] public float health;
    [HideInInspector] public float shield;

    [SerializeField, Range(0, 20)] float shieldRegenRate;
    [SerializeField, Range(0, 5)] float shieldRegenCooldown;
    bool canRegenerate;

    HUD hud;
    Animator anim;

    public int granadeAmmo;

    [SerializeField] AudioClip shieldDamage;
    [SerializeField] AudioClip shieldRegenSFX;
    [SerializeField] AudioClip healthDamage;

    bool shieldRegenStart;
    [SerializeField] ParticleSystem deathFX;

    Animator canvasAnim;
    [SerializeField] AudioClip gameOverMusic;

    private void Awake()
    {
        hud = GameObject.Find("Canvas").GetComponent<HUD>();
        canvasAnim = hud.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        shield = maxShield;
        hud.UpdateBars();
    }

    private void Update()
    {
        if (canRegenerate)
        {
            if (!shieldRegenStart)
                AudioManager.Instance.PlaySFX(shieldRegenSFX);

            RegenerateShield();
        }
    }

    public void TakeDamage(float damage)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Normal"))
        {
            if (shield <= 0)
            {
                health -= damage;
                anim.SetTrigger("Damaged");
                CinemachineShake.Instance.ShakeCamera(6f, 0.1f);
                AudioManager.Instance.PlaySFX(healthDamage);
            }
            else
            {
                shield -= damage;
                anim.SetTrigger("ShieldBlock");
                CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
                AudioManager.Instance.PlaySFX(shieldDamage);
            }
        }

        if (shield < 0)
            shield = 0;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        canRegenerate = false;
        StopAllCoroutines();

        hud.UpdateBars();

        StartCoroutine(RegenerateShieldCooldown());
    }

    void RegenerateShield()
    {
        shieldRegenStart = true;

        if (shield < maxShield)
        {
            shield += shieldRegenRate * Time.deltaTime;
        }
        else
        {
            shield = maxShield;
            AudioManager.Instance.StopSFX();
            canRegenerate = false;
        }
        hud.UpdateBars();
    }

    IEnumerator RegenerateShieldCooldown()
    {
        yield return new WaitForSeconds(shieldRegenCooldown);

        canRegenerate = true;
        shieldRegenStart = false;
    }

    public void Heal(float amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        hud.UpdateBars();
    }

    void Die()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        deathFX.Play();
        deathFX.transform.parent = null;
        canvasAnim.SetTrigger("GameOver");
        AudioManager.Instance.PlayMusicWithFade(gameOverMusic, false);
        Destroy(gameObject);
    }

    public void DialogueStarted()
    {
        GetComponent<Movement>().rb.velocity = Vector3.zero;
        anim.SetInteger("Speed", 0);
        anim.SetTrigger("DialogueStart");
    }
}
