using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour
{
    Rigidbody2D rb;

    public bool canGlide;
    [SerializeField, Range(0, 5)] float glideFallVelocity;
    bool gliding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Planar
        if (!canGlide)
            return;

        if (rb.velocity.y < 0 && Input.GetButton("Jump") && canGlide)
        {
            gliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -glideFallVelocity);
        }
        else
            gliding = false;
    }
}
