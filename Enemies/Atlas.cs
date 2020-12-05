using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atlas : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject rock;

    Animator anim;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] float rockFireRate;
    bool canShot = true;

    [SerializeField] float pauseTime;
    public bool canAct = true;

    [SerializeField] Pillar pillars1;
    [SerializeField] Pillar pillars2;
    [SerializeField] Pillar pillars3;

    public bool start;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Started", start);

        if (target && start)
        {
            if (canAct)
                ChooseAction();
            else
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("ThrowRocks") && canShot)
                {
                    StartCoroutine(RockCooldown());
                    Transform firePoint = firePoints[Random.Range(0, firePoints.Count)];

                    Vector3 dir = target.position - firePoint.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    firePoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(rock, firePoint.position, firePoint.rotation);
                    canShot = false;
                }
            }
        }
    }

    IEnumerator RockCooldown()
    {
        yield return new WaitForSeconds(rockFireRate);
        canShot = true;
    }

    void ChooseAction()
    {
        canAct = false;

        float rng = Random.value;

        if (rng < 0.5f)
        {
            float jumpRng = Random.value;
            if (jumpRng == 0.5f)
                jumpRng = 0.1f;
            anim.SetTrigger("LowJump");
            anim.SetFloat("Random", jumpRng);
            StartCoroutine(ActionCooldown());
            return;
        }
        if (rng >= 0.5f)
        {
            anim.SetTrigger("Special");
            StartCoroutine(ActionCooldown());
            return;
        }
    }

    IEnumerator ActionCooldown()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        canShot = true;
        yield return new WaitForSeconds(pauseTime);
        canAct = true;
        StopAllCoroutines();
    }

    public void Pillar1On()
    {
        pillars1.DangerOn();
    }

    public void Pillar2On()
    {
        pillars2.DangerOn();
    }

    public void Pillar3On()
    {
        pillars3.DangerOn();
    }

    public void Pillar1Off()
    {
        pillars1.DangerOff();
    }

    public void Pillar2Off()
    {
        pillars2.DangerOff();
    }

    public void Pillar3Off()
    {
        pillars3.DangerOff();
    }

    public void PillarMoving()
    {
        target.GetComponent<Grab>().ReleaseEdge();
    }

}
