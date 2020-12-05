using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [SerializeField, Range(5, 20)] float jumpVelocity;
    [SerializeField, Range(0, 0.5f)] float jumpBufferLength;
    float jumpBufferCounter = 0;

    bool isGrounded = false;
    LayerMask groundLayer;

    [SerializeField, Range(0, 0.5f)] float coyoteTimeLength;
    float coyoteTimeCounter;

    public int extraJumps;
    int remainingJumps;

    [SerializeField, Range(0, 10)] float fallGravityMultiplier;
    [SerializeField, Range(0, 10)] float lowJumpGravityMultiplier;

    Grab grab;

    [SerializeField] ParticleSystem dustFX;

    [SerializeField] AudioClip landingSFX;
    bool lastFrameIsGrounded;
    float lastFrameYVelocity;

    bool falling;
    float airTime;

    [SerializeField] AudioClip jumpSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grab = GetComponent<Grab>();
        groundLayer = LayerMask.GetMask("Ground");
        remainingJumps = extraJumps;
    }

    private void LateUpdate()
    {
        lastFrameYVelocity = rb.velocity.sqrMagnitude;
        lastFrameIsGrounded = isGrounded;
    }

    private void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().paused)
            return;

        //Monitora se o player está no chão
        isGrounded = Physics2D.OverlapCircle(transform.position - transform.up * 0.85f, 0.35f, groundLayer);

        //Input Buffer do Pulo - O personagem ira pular mesmo se o player apertar o botão um pouco antes de chegar no chão
        jumpBufferCounter = Input.GetButtonDown("Jump") ? jumpBufferLength : jumpBufferCounter - Time.deltaTime;

        //Coyote Time - O personagem ira pular mesmo se o player apertar o botão um pouco depois de sair da plataforma
        coyoteTimeCounter = isGrounded ? coyoteTimeLength : coyoteTimeCounter - Time.deltaTime;

        //Input
        if (!GameObject.Find("GameController").GetComponent<GameController>().inDialogue)
            if (jumpBufferCounter >= 0 && coyoteTimeCounter > 0 || jumpBufferCounter >= 0.05f && remainingJumps > 0 && !grab.grabbing)
            {
                if (isGrounded)
                    dustFX.Play();
                //else if (extraJumps > 0)
                    //AudioManager.Instance.PlaySFX(jumpSFX);

                rb.velocity = Vector2.up * jumpVelocity;
                jumpBufferCounter = 0;

                remainingJumps--;
            }

        if (coyoteTimeCounter > 0)
            remainingJumps = extraJumps;

        //Suavizar pulo
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime;

        //animação
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("YVelocity", rb.velocity.y);

        //Aterrissagem
    }

    void CheckAirTime()
    {
        if (isGrounded)
            airTime = 0f;
        else
            airTime += Time.deltaTime;
    }

    void CheckLand()
    {
        if (airTime > 0.9f)
            if (isGrounded)
                Land();
    }

    void Land()
    {
        AudioManager.Instance.PlaySFX(landingSFX);
        falling = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - transform.up * 0.85f, 0.35f);
    }
}
