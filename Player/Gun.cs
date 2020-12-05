using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool granadeLauncher;
    [SerializeField] GameObject granade;
    [SerializeField] Transform firePoint;
    [SerializeField] SpriteRenderer gun;
    [SerializeField] AudioClip shotSFX;

    bool canShot = true;
    [SerializeField] float fireRate;

    void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().paused ||
            GameObject.Find("GameController").GetComponent<GameController>().inDialogue)
            return;

        Aim();
        if (granadeLauncher && Input.GetMouseButtonDown(1) && GameObject.FindObjectOfType<Player>().granadeAmmo > 0 && canShot)
            Shoot(granade);
    }

    void Shoot(GameObject shot)
    {
        Instantiate(shot, firePoint.position, firePoint.rotation);
        GameObject.FindObjectOfType<Player>().granadeAmmo--;
        GameObject.FindObjectOfType<HUD>().UpdateBars();
        StartCoroutine(ShotCooldown());
    }

    IEnumerator ShotCooldown()
    {
        canShot = false;
        yield return new WaitForSeconds(fireRate);
        canShot = true;
    }

    void Aim()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (transform.parent.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            gun.flipY = true;
            transform.position = transform.parent.position - transform.up * 0.05f;
        }
        else
        {
            gun.flipY = false;
            transform.position = transform.parent.position;
        }

    }
}
