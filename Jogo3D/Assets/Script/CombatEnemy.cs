using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatEnemy : MonoBehaviour
{
    [Header("Atributes")]
    public float totalDamage;
    public float attackDamage;
    public float movementSpeed;
    public float lookRadius;

    [Header("Components")]
    private Animation anim;
    private CapsuleCollider capsule;
    private NavMeshAgent Agent;

    [Header("Others")] 
    private Transform player;
    void Start()
    {
        anim = GetComponent<Animation>();
        capsule = GetComponent<CapsuleCollider>();
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

   
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if ((distance <= lookRadius))
        {
            Agent.SetDestination(player.position);
            //O personagem esta no raio de ação
            if (distance <= Agent.stoppingDistance)
            {
                Debug.Log("ATACAR");
            }
        }
        else
        {
            //O personagem não esta no raio de ação
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
