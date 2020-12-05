using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grab : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [SerializeField, Range(0, 2)] float grabDistance;

    Vector3 edgePos = Vector3.zero;
    public bool grabbing;
    [SerializeField] LayerMask groundLayer;
    float startGravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startGravity = rb.gravityScale;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Detecta a beirada
        RaycastHit2D upHit = Physics2D.Raycast(transform.position + transform.up * 1.85f, transform.right * Input.GetAxisRaw("Horizontal"), grabDistance * 2, groundLayer);
        RaycastHit2D middleHit = Physics2D.Raycast(transform.position + transform.up * 1.55f, transform.right * Input.GetAxisRaw("Horizontal"), grabDistance, groundLayer);

        if (middleHit.collider != null && upHit.collider == null && !grabbing)
        {
            if (middleHit.collider.GetComponent<Tilemap>() != null)
            {
                edgePos = middleHit.collider.GetComponent<Tilemap>().WorldToCell(middleHit.point + Vector2.up + Vector2.right);
                edgePos = middleHit.collider.GetComponent<Tilemap>().CellToWorld(Vector3Int.RoundToInt(edgePos));
            }
            else
            {
                edgePos = GetClosestGrabPoint();
            }
            GrabEdge();
        }

        if (grabbing)
        {
            //Trava a movimentação do player
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;

            //Solta a beirada ao apertar na direção oposta ou para baixo
            if (transform.position.x > edgePos.x && Input.GetAxisRaw("Horizontal") > 0 ||
                transform.position.x < edgePos.x && Input.GetAxisRaw("Horizontal") < 0 ||
                Input.GetAxisRaw("Vertical") < 0)
                ReleaseEdge();

            //Sobe a beirada se o player apertar o botão novamente
            if (transform.position.x > edgePos.x && Input.GetKeyDown(KeyCode.A) ||
                transform.position.x < edgePos.x && Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Jump"))
                anim.SetTrigger("ClimbEdge");

            //Sobe a beirada se o player continuar segurando o botão por um tempo
            if (transform.position.x > edgePos.x && Input.GetKey(KeyCode.A) ||
                transform.position.x < edgePos.x && Input.GetKey(KeyCode.D))
                StartCoroutine(ClimbDelay());
            else
                StopAllCoroutines();
        }
    }

    void GrabEdge()
    {
        anim.SetTrigger("GrabEdge");
        grabbing = true;
        transform.position = edgePos - (transform.right * Input.GetAxisRaw("Horizontal") / 3) - transform.up * 1.75f;
    }

    public void ReleaseEdge()
    {
        grabbing = false;
        anim.SetTrigger("ReleaseEdge");
        rb.gravityScale = startGravity;
    }

    IEnumerator ClimbDelay()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger("ClimbEdge");
    }

    void ClimbEdge()
    {
        ReleaseEdge();
        transform.position = edgePos + transform.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(edgePos, 0.5f);

        Gizmos.DrawRay(transform.position + transform.up * 1.85f, transform.right * Input.GetAxisRaw("Horizontal") * grabDistance * 2);

        Gizmos.DrawRay(transform.position + transform.up * 1.55f, transform.right * Input.GetAxisRaw("Horizontal") * grabDistance);
    }

    Vector3 GetClosestGrabPoint()
    {
        Vector3 closestGrabPoint = transform.position;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in GameObject.FindGameObjectsWithTag("GrabPoint"))
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestGrabPoint = potentialTarget.transform.position;
            }
        }
        return closestGrabPoint;
    }

}
