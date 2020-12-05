using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float damage;

    public float force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Enemy")
        {
            if (other.tag == "Player")
                other.GetComponent<Player>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
