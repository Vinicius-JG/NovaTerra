using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;

    [SerializeField, Range(1, 40)] float speed;

    Grab grab;
    public AudioClip footstepSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        grab = GetComponent<Grab>();
    }

    private void Update()
    {
        if (GameObject.Find("GameController").GetComponent<GameController>().paused ||
            GameObject.Find("GameController").GetComponent<GameController>().inDialogue)
            return;
        //Andar
        if (!grab.grabbing)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

            //Controlar flip de animações
            if (rb.velocity.x > 0)
                FlipRight();

            if (rb.velocity.x < 0)
                FlipLeft();
        }

        //Passa a direção do movimento para o animator
        anim.SetInteger("Speed", Mathf.FloorToInt(rb.velocity.x));
    }

    public void PlayFootstepSFX()
    {
        AudioManager.Instance.PlaySFX(footstepSFX);
    }

    public void FlipRight()
    {
        spriteRenderer.sortingLayerName = "PlayerRight";
        anim.runtimeAnimatorController = Resources.Load("Player") as RuntimeAnimatorController;
    }

    public void FlipLeft()
    {
        spriteRenderer.sortingLayerName = "PlayerLeft";
        anim.runtimeAnimatorController = Resources.Load("PlayerAlternative") as RuntimeAnimatorController;
    }
}
