using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    Transform target;
    bool aggro;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float walkDistance;
    float startXPos;
    bool flip;
    bool attacking;
    [SerializeField] float chargeTime;
    [SerializeField] float shotCooldown;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    Animator anim;

    [SerializeField] AudioClip turtleSFX;
    [SerializeField] LayerMask groundLayer;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startXPos = transform.position.x;
    }

    private void Update()
    {
        if (target)
            aggro = Vector2.Distance(transform.position, target.position) < range;
        else
            aggro = false;

        if (aggro)
        {
            if (!attacking)
                StartCoroutine(AttackCharge());

            rb.velocity = Vector2.zero;
            anim.SetBool("Walking", false);
        }
        else
        {
            Wander();
        }
    }

    IEnumerator AttackCharge()
    {
        attacking = true;
        yield return new WaitForSeconds(chargeTime);
        Attack();
    }

    void Attack()
    {
        LookToPlayer();

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        projectile.GetComponent<Bullet>().force = Vector2.Distance(transform.position, target.position) * 400;
        Instantiate(projectile, firePoint.position, firePoint.rotation);
        attacking = false;
        AudioManager.Instance.PlaySFX(turtleSFX);
    }

    void LookToPlayer()
    {
        if (target.position.x > transform.position.x)
            firePoint.eulerAngles = new Vector3(firePoint.eulerAngles.x, 0, firePoint.eulerAngles.z);
        else
            firePoint.eulerAngles = new Vector3(firePoint.eulerAngles.x, 180, firePoint.eulerAngles.z);
    }

    void Wander()
    {
        anim.SetBool("Walking", true);

        if (transform.position.x > startXPos + walkDistance)
            flip = true;

        if (transform.position.x < startXPos - walkDistance)
            flip = false;

        if (!flip)
            rb.velocity = transform.right * speed * Time.deltaTime;
        else
            rb.velocity = -transform.right * speed * Time.deltaTime;
    }
}
