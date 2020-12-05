using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Cinemachine;

public class FlashlightLaser : MonoBehaviour
{
    [SerializeField] Light2D flashlight;
    [SerializeField] Light2D globalLight;

    float startInnerAngle;
    float startOuterAngle;
    float startInnerRadius;
    float startOuterRadius;
    float startIntensity;
    Color startColor;

    float globalStartIntensity;
    Color globalStartColor;

    [SerializeField] float cooldown;
    bool canShot = true;

    [SerializeField] float speed;
    [SerializeField] float laserTime;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject secondaryLaser;

    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] Transform camTarget;
    [HideInInspector] public bool charging = false;

    [SerializeField] float damage;

    [SerializeField] AudioClip chargeSFX;
    [SerializeField] AudioClip laserSFX;

    private void Awake()
    {
        StoreStartValues();
        globalLight.intensity = globalStartIntensity;
        globalLight.color = globalStartColor;
    }

    private void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().paused ||
            GameObject.Find("GameController").GetComponent<GameController>().inDialogue)
            return;

        if (Input.GetMouseButtonDown(0) && !laser.activeSelf && !charging && flashlight.gameObject.activeSelf)
        {
            AudioManager.Instance.PlaySFX(chargeSFX);
            charging = true;
        }

        if (charging && flashlight.pointLightOuterAngle > 0 && !laser.activeSelf && canShot)
            ChargeLaser();

        if (Input.GetMouseButtonUp(0) && !laser.activeSelf)
        {
            AudioManager.Instance.StopSFX();
            ResetFlashlight();
        }


        if (flashlight.pointLightOuterAngle <= 0 && !laser.activeSelf)
            ShotLaser();

        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laser.transform.up);

        float laserSize = hit.distance;

        if (laserSize > 30)
            laserSize = 30;

        float laserLength = (40 - laserSize) / 2;

        if (hit.collider)
        {
            laser.GetComponent<Light2D>().pointLightOuterAngle = laserLength;
            secondaryLaser.GetComponent<Light2D>().pointLightOuterAngle = laserLength;
            laser.GetComponent<Light2D>().pointLightInnerRadius = laserSize;
            laser.GetComponent<Light2D>().pointLightOuterRadius = laserSize;
            secondaryLaser.GetComponent<Light2D>().pointLightInnerRadius = laserSize;
            secondaryLaser.GetComponent<Light2D>().pointLightOuterRadius = laserSize;
            secondaryLaser.transform.localPosition = new Vector3(laserSize - laser.transform.localPosition.x + 0.35f, secondaryLaser.transform.localPosition.y, secondaryLaser.transform.localPosition.z);
        }

        cam.Follow = Input.GetKey(KeyCode.LeftShift) ? camTarget : transform;
    }

    void ChargeLaser()
    {
        flashlight.pointLightInnerAngle -= speed * Time.deltaTime;
        flashlight.pointLightOuterAngle -= speed * Time.deltaTime;
        flashlight.pointLightInnerRadius += speed * Time.deltaTime / 20;
        flashlight.pointLightOuterRadius += speed * Time.deltaTime / 20;
        flashlight.intensity += speed * Time.deltaTime / 3.5f;
        flashlight.color -= new Color(0, 0, Time.deltaTime / 1.2f, 0);
        charging = true;
    }

    void ShotLaser()
    {
        canShot = false;
        StartCoroutine(ShotCooldown());
        laser.SetActive(true);
        secondaryLaser.SetActive(true);
        CinemachineShake.Instance.ShakeCamera(3f, laserTime);
        globalLight.intensity = 0.1f;
        StartCoroutine(LaserTime());

        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laser.transform.up);

        if (hit.collider)
        {
            if (hit.collider.tag == "Enemy")
                hit.collider.GetComponent<EnemyStats>().TakeDamage(damage);
            if (hit.collider.tag == "Rock")
                Destroy(hit.collider.gameObject);
        }

        AudioManager.Instance.PlaySFX(laserSFX);
    }

    void StopLaser()
    {
        laser.SetActive(false);
        secondaryLaser.SetActive(false);
        globalLight.intensity = globalStartIntensity;
        globalLight.color = globalStartColor;
        ResetFlashlight();
    }

    IEnumerator LaserTime()
    {
        yield return new WaitForSeconds(laserTime);
        StopLaser();
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShot = true;
    }

    void StoreStartValues()
    {
        startInnerAngle = flashlight.pointLightInnerAngle;
        startOuterAngle = flashlight.pointLightOuterAngle;
        startInnerRadius = flashlight.pointLightInnerRadius;
        startOuterRadius = flashlight.pointLightOuterRadius;
        startIntensity = flashlight.intensity;
        startColor = flashlight.color;

        globalStartIntensity = globalLight.intensity;
        globalStartColor = globalLight.color;
    }

    void ResetFlashlight()
    {
        flashlight.pointLightInnerAngle = startInnerAngle;
        flashlight.pointLightOuterAngle = startOuterAngle;
        flashlight.pointLightInnerRadius = startInnerRadius;
        flashlight.pointLightOuterRadius = startOuterRadius;
        flashlight.intensity = startIntensity;
        flashlight.color = startColor;
        charging = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(laser.transform.position, laser.transform.up);
    }
}
