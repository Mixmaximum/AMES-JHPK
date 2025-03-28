using UnityEngine;

public class SlideLegsToggle : MonoBehaviour
{
    [Header("Leg Mesh")]
    public GameObject legsMesh;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    void Start()
    {
        if (legsMesh != null)
        {
            legsMesh.SetActive(false); // Hide legs by default
        }
    }

    void Update()
    {
        // Check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (legsMesh == null) return;

        // Show legs only if holding C and grounded
        if (Input.GetKey(KeyCode.C) && isGrounded)
        {
            legsMesh.SetActive(true);
        }
        else
        {
            legsMesh.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
