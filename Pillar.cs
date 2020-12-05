using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] float damage;
    bool danger = false;

    public void DangerOn()
    {
        danger = true;
    }

    public void DangerOff()
    {
        danger = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" && danger)
        {
            other.collider.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
