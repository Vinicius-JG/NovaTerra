using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Transform target;
    Animator anim;
    [SerializeField] float range;

    [SerializeField] AudioClip batSFX;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target)
        {
            if (Vector2.Distance(transform.position, target.position) < range && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
            {
                LookAtPlayer();
                anim.SetTrigger("Attack");
                AudioManager.Instance.PlaySFX(batSFX);
            }
        }
    }

    void LookAtPlayer()
    {
        if (transform.position.x > target.position.x)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }
}
