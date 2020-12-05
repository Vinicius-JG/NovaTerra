using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] AudioClip sfx;

    private void Start()
    {
        AudioManager.Instance.PlaySFX(sfx);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            other.GetComponent<Player>().TakeDamage(25f);

        if (other.tag == "Enemy")
            other.GetComponent<EnemyStats>().TakeDamage(100f);

        if (other.tag == "Rock")
            Destroy(other.gameObject);
    }
}
