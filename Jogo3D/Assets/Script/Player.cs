using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private CharacterController controller;
    public float totalHealth = 100;
    private Transform cam;
    private Vector3 moveDirection;
    public float gravity;
    public float damage = 20;
    public float ColliderRadius;

    private Animator anim;


    public float smoothRotTime;
    private float turnSmoothVelocity;
    public List<Transform> enemyList = new List<Transform>();
    private bool isWalking;
    private bool waitFor;
    private bool isHitting;

    public bool isDead;

    public AudioSource PLayerAtacando;
    public AudioSource PlayerMorrendo;
    public AudioSource Coin;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        
        GameController.instance.Coracao(totalHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            GetMouseInput();
        }
        
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
                if (!anim.GetBool("Attacking"))
                {
                    //armazena a rotação e o angulo da camera
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                    //aemazena a rotação mais suave
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                    //rotaciona o personagem
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    //armazena a direção
                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;

                    anim.SetInteger("Transitions", 1);
                    isWalking = true;
                }
                else
                {
                    moveDirection = Vector3.zero;
                    anim.SetBool("Walking", false);
                }

            }
            else if (isWalking)
            {
                anim.SetInteger("Transitions", 0);
                anim.SetBool("Walking", false);
                moveDirection = Vector3.zero;
                isWalking = false;
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
                if (anim.GetBool("Walking"))
                {
                    anim.SetBool("Walking", false);
                    anim.SetInteger("Transitions", 0);
                }

                if (!anim.GetBool("Walking"))
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!waitFor && !isHitting)
        {
            PLayerAtacando . Play();
            waitFor = true;
            anim.SetBool("Attacking", true);
            anim.SetInteger("Transitions", 2);
            yield return new WaitForSeconds(1.2f);
            GetEnemeslist();

            foreach (Transform e in enemyList)
            {
                //dano no inimigo
                CombatEnemy enemy = e.GetComponent<CombatEnemy>();

                if (enemy != null)
                {
                    enemy.GetHit(damage);
                }
            }

        }

        yield return new WaitForSeconds(1f);
        anim.SetInteger("Transitions", 0);
        anim.SetBool("Attacking", false);
        waitFor = false;


    }

    void GetEnemeslist()
    {
        enemyList.Clear();
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
        {
            if (c.gameObject.CompareTag("Enemy"))
            {
                enemyList.Add(c.transform);
            }
        }
    }

    public void GetHit(float damage)
    {
        totalHealth -= damage;
        PlayerMorrendo . Play();
        GameController.instance.Coracao(totalHealth);
        if (totalHealth > 0)
        {
            //esta vivo
            StopCoroutine("Attack");
            anim.SetInteger("Transitions", 3);
            isHitting = true;
            StartCoroutine("RecorveryHit");
        }
        else
        {
            //player esta morto
            isDead = true;
            anim.SetTrigger("die");
        }
    }

    public void IncreaseHealth(float value)
    {
        totalHealth += value;
        GameController.instance.Coracao(totalHealth);
    }
    
    IEnumerator RecorveryHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Transitions", 0);
        isHitting = false;
        anim.SetBool("Attacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, ColliderRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Coin.Play();
        }
    }
}
                                                                                                                                                                                                                   