using UnityEngine;
using UnityEngine.AI;

public class bear_attack : MonoBehaviour
{
    [Header("Bear AI Settings")]
    public float detectionRadius = 10f;
    public float attackDistance = 2f;
    public int attackDamage = 35;
    public float attackCooldown = 2f;

    private float attackTimer = 0f;

    [Header("References")]
    public Transform player;
    public BearHealth health;
    public Animator animator;
    public NavMeshAgent agent;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("XRPlayer not assigned! Please drag the XRPlayer into the 'player' field.");
            enabled = false;
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        Debug.Log(" Player reference successfully assigned.");
    }

    void Update()
    {
        if (health.currentHealth <= 0) return;   // Bear dead

        attackTimer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        Debug.Log(dist);
        // Player too far away stop moving
        if (dist > detectionRadius)
        {
            animator.SetBool("Run Forward", false);
            agent.ResetPath();
            return;
        }

        ChasePlayer(dist);
    }

    void ChasePlayer(float dist)
    {
        
        if (dist > attackDistance)
        {
            // Clear attack triggers so Animator exits attack state
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");

            // Running animation
            animator.SetBool("Run Forward", true);

            // Move toward player
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Smooth horizontal rotation toward player
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
            }

            return; // prevents attack logic from running
        }

        
        agent.isStopped = true;
        AttackPlayer();

        // Keep facing player but stay upright
        Vector3 look = player.position;
        look.y = transform.position.y;
        transform.LookAt(look);
    }

    void AttackPlayer()
    {
        animator.SetBool("Run Forward", false);

        if (attackTimer <= 0f)
        {
            // Random attack animation
            string attackAnim = Random.value > 0.5f ? "Attack1" : "Attack2";
            animator.SetTrigger(attackAnim);

            // Apply damage
            player.GetComponent<Player_H>().TakeDamage(attackDamage);

            attackTimer = attackCooldown;
        }
    }
}
