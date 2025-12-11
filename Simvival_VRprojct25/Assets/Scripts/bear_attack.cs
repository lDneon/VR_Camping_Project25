using UnityEngine;
using UnityEngine.AI;

public class bear_attack : MonoBehaviour
{
    public float detectionRadius = 20f;
    public float attackDistance = 2f;
    public int attackDamage = 35;
    public float attackCooldown = 2f;

    private float attackTimer = 0f;

    public Transform player;
    public BearHealth health;
    public Animator animator;
    public NavMeshAgent agent;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("XRPlayer")?.transform;
            if (player == null)
            {
                Debug.LogError("❌ XRPlayer not found!");
                enabled = false;
                return;
            }
        }

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (health.currentHealth <= 0) return;

        attackTimer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > detectionRadius)
        {
            animator.SetBool("Run Forward", false);
            agent.isStopped = true;
            agent.ResetPath();
            return;
        }

        ChasePlayer(dist);
    }

    void ChasePlayer(float dist)
    {
        if (dist > attackDistance)
        {
            animator.SetBool("Run Forward", true);

            agent.isStopped = false;
            agent.SetDestination(player.position);

            Vector3 look = player.position - transform.position;
            look.y = 0;
            if (look != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 5);

            return;
        }

        agent.isStopped = true;

        Vector3 lp = player.position;
        lp.y = transform.position.y;
        transform.LookAt(lp);

        AttackPlayer();
    }

    void AttackPlayer()
    {
        animator.SetBool("Run Forward", false);

        if (attackTimer <= 0f)
        {
            string atk = Random.value > 0.5f ? "Attack1" : "Attack2";
            animator.SetTrigger(atk);

            player.GetComponent<Player_H>().TakeDamage(attackDamage);

            attackTimer = attackCooldown;
        }
    }
}
