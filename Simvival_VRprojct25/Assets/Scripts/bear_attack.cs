using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class bear_attack : MonoBehaviour
{
    // Start is called before the first frame update

    public float detectionRadius = 10f;
    public float attackDistance = 2f;
    public int attackDamage = 35;
    public float attackCooldown = 2f;


    private float attackTimer = 0f;
    public Transform player;
    public BearHealth health;
    public Animator animator;

    private NavMeshAgent agent;



    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
           
   
    }
    private void Update()
    {
        if (health.currentHealth <= 0) return;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > detectionRadius)
        {
            animator.SetBool("Run Forward", false);
            agent.ResetPath();
            return;
        }

        ChasePlayer();
    }
    void ChasePlayer()
    {
        animator.SetBool("Run Forward", true);
        agent.SetDestination(player.position);
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackDistance)
        {
            agent.ResetPath();
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        animator.SetBool("Run Forward", true);

        if(attackTimer <= 0f)
        {
            animator.SetTrigger(Random.value > 0.5f ? "Attack1" : "Attack2");
            player.GetComponent<Player_H>().TakeDamage(attackDamage);

            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }
}


