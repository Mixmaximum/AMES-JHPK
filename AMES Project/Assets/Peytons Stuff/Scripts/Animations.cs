using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public float speed; // Speed of the player
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // Check if the Rigidbody2D velocity has a significant magnitude (indicating movement)
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            // Play the walk animation
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop the walk animation
            animator.SetBool("isWalking", false);
        }
    }
}
