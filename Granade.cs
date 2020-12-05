using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField, Range(500, 10000)] float force;
    [SerializeField] GameObject explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * force);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Destructible")
        {
            print("Ok");
            Destroy(other.gameObject);
        }

        if (other.tag != "Player" && other.tag != "Explosion" && other.tag != "AreaCollider")
            Explode();
    }

}
