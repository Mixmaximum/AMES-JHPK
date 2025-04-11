using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator enemyAnimator;        // Set this in Inspector
    public float detectionRadius = 10f;   // You can change this in the Inspector
    public GameObject fallColliderObject; // Set this in Inspector to specify the collider object

    private NavMeshAgent agent;
    private bool isFalling = false;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Don't do anything if the enemy is falling or playing the "Fall" animation
        if (isFalling || IsPlayingAnimation("Fall")) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Only chase the player if within detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;
            enemyAnimator.SetFloat("Walking", speed);
            enemyAnimator.SetBool("Attacking", distanceToPlayer < 2f);

            // Keep upright (X = -90) and rotate to face the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(-90f, lookRotation.eulerAngles.y, 0f);
            }
        }
        else
        {
            // Out of range, stop moving
            agent.SetDestination(transform.position);
            enemyAnimator.SetFloat("Walking", 0f);
            enemyAnimator.SetBool("Attacking", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slide")) // Check if the enemy collided with an object tagged "Slide"
        {
            Fall(); // Trigger the fall animation and logic
        }
    }

    public void Fall()
    {
        if (isFalling) return; // Avoid repeating fall action if already falling

        isFalling = true;
        enemyAnimator.SetBool("IsFalling", true); // Play the fall animation
        agent.enabled = false; // Disable NavMeshAgent so it doesn't keep moving

        // Optionally, you can recover the enemy after a delay (e.g., 2 seconds)
        Invoke(nameof(Recover), 2f); // Recover after 2 seconds or as needed
    }

    void Recover()
    {
        isFalling = false;
        enemyAnimator.SetBool("IsFalling", false); // Stop the fall animation
        agent.enabled = true; // Re-enable NavMeshAgent for movement
    }

    bool IsPlayingAnimation(string animationName)
    {
        if (enemyAnimator == null) return false;
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
