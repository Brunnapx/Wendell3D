using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatEnemy : MonoBehaviour
{
    [Header("Atributes")]
    public float totalHealth = 100;
    public float attackDamage;
    public float movementSpeed;
    public float lookRadius;
    public float colliderRadius = 2;
    public float rotationSpeed;

    [Header("Components")]
    private Animator anim;
    private CapsuleCollider capsule;
    private NavMeshAgent Agent;

    [Header("Others")] 
    private Transform player;

    private bool walking;
    private bool attacking;
    private bool Hiting;
    private bool waitfor;
    private bool playerIsDead;
    void Start()
    {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (totalHealth > 0)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if ((distance <= lookRadius))
            {
                //O personagem esta no raio de ação
                Agent.isStopped = false;
                if (!attacking)
                {
                    Agent.SetDestination(player.position);
                    anim.SetBool("Walk Forward", true);
                    walking = true;
                }

                if (distance <= Agent.stoppingDistance)
                {
                    StartCoroutine("Attack");
                    LookTarget();
                }
                else
                {
                    attacking = false;
                }
            }
            else
            {
                //O personagem não esta no raio de ação
                anim.SetBool("Walk Forward", false);
                Agent.isStopped = true;
                walking = false;
                attacking = false;

            }

        }
    }
    
    IEnumerator Attack()
    {
        if (!waitfor && !Hiting)
        {
            waitfor = true;
            attacking = true;
            walking = false;
            anim.SetBool("Web Attack", true);
            anim.SetBool("Walk Forward", false);
            yield return new WaitForSeconds(1f);
            GetPlayer();
            yield return new WaitForSeconds(1f);
            waitfor = false;
        }

        if (player.GetComponent<Player>().isDead)
        {
            
        }
    }

    void GetPlayer()
    {
        foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius))
        {
            if (c.gameObject.CompareTag("Player"))
            {
              //vai causar dano do player
              c.gameObject.GetComponent<Player>().GetHit(attackDamage);
              playerIsDead = c.gameObject.GetComponent<Player>().isDead;
            }
        }
    }

    public void GetHit(float damage)
    {
        totalHealth -= damage;
        if (totalHealth > 0)
        {
            //esta vivo
            StopCoroutine("Attack");
            anim.SetTrigger("Take Damage");
            Hiting = true;
            StartCoroutine("RecorveryFromHit");
        }
        else
        {
            //esta morto
            anim.SetTrigger("Die");
        }
    }

    IEnumerator RecorveryFromHit()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Walk Forward", false);
        anim.SetBool("Web Attack", false);
        Hiting = false;
        waitfor = false;
    }

    void LookTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
