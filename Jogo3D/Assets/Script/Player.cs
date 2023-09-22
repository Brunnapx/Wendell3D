using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private CharacterController controller;
    private Transform cam;
    private Vector3 moveDirection;
    public float gravity;

    public float ColliderRadius;
    
    private Animator anim;


    public float smoothRotTime;
    private float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetMouseInput();
    }


    private void Move()
    {
        if (controller.isGrounded)
        {
            //pegar a entrada na horizontal (Tecla direita/esquerda)
            float horizontal = Input.GetAxisRaw("Horizontal");
            //pegar a entrada na horizontal (Tecla Cima/Baixo)
            float vertical = Input.GetAxisRaw("Vertical");
            // Variavel local que armazena o valor de eixo horizontal e vertical
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            //verifica se o personagem esta se movimentando (se for > 0) 
            if (direction.magnitude > 0)
            {
                //armazena a rotação e o angulo da camera
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                //aemazena a rotação mais suave
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity,
                    smoothRotTime);

                //rotaciona o personagem
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //armazena a direção
                moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;
                
                anim.SetInteger("Transitions", 1);

            }
            else
            {
                moveDirection = Vector3.zero;
                anim.SetInteger("Transitions", 0);
            }

        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
                anim.SetInteger("Transitions", 2);
            }
        }
    }

    IEnumerator Attack()
    {
        anim.SetInteger("Transitions", 2);
        yield return new WaitForSeconds(1f);
    }

    void GetEnemEslist()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+transform.forward, ColliderRadius);
    }
}