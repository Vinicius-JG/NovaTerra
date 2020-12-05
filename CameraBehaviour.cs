using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    Transform target;

    Animator anim;

    [SerializeField] float smoothTime;

    [SerializeField] Transform shiftTarget;

    [SerializeField] Transform camTarget;
    [SerializeField, Range(0, 10)] float camAheadAmount;
    [SerializeField, Range(0, 30)] float camAheadSpeed;

    private void Start()
    {
        target = camTarget;
        //anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (target != null)
        {
            //Muda o alvo da câmera baseado em se o player está segurando o botão ou não
            target = Input.GetKey(KeyCode.LeftShift) ? shiftTarget : camTarget;

            //Faz a camera ficar sempre um pouco a frente do player enquanto ele anda
            if (Input.GetAxisRaw("Horizontal") != 0)
                camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, camAheadAmount * Input.GetAxisRaw("Horizontal"), camAheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }

    void FixedUpdate()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position - Vector3.forward * 10, smoothTime);
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }
}
