using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashlightBehaviour : MonoBehaviour
{
    [SerializeField] Light2D flashlight;
    float mouseDistance;
    FlashlightLaser laser;

    private void Awake()
    {
        laser = GetComponent<FlashlightLaser>();
    }

    private void Update()
    {
        if (!laser.charging)
        {
            mouseDistance = Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            flashlight.pointLightOuterRadius = mouseDistance * 1.3f;
            flashlight.pointLightOuterAngle = 90;
        }
    }
}
