using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator enemyAnimator;
    public float detectionRadius = 10f;
    public Transform visual; // Assign your visual/mesh child here in the Inspector

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsPlayingAnimation("Fall")) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            enemyAnimator.SetFloat("Walking", speed);
            enemyAnimator.SetBool("Attacking", distanceToPlayer < 2f);

            // Rotate the visual (not the NavMeshAgent parent) to face the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                visual.rotation = Quaternion.Slerp(visual.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            enemyAnimator.SetFloat("Walking", 0f);
            enemyAnimator.SetBool("Attacking", false);
        }
    }

    bool IsPlayingAnimation(string animationName)
    {
        if (enemyAnimator == null) return false;
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
