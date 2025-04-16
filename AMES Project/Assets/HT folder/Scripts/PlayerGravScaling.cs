using UnityEngine;

public class PlayerGravScaling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovement pm;
    [SerializeField] WallRun wallRun;
    [SerializeField] Rigidbody rb;

    [Header("Falling Settings")]
    [SerializeField] float fallingGrav;
    [SerializeField] float maxFallGrav;
    [SerializeField] float timeToMaxGrav;

    float currentFallGrav;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (maxFallGrav < fallingGrav)
        {
            maxFallGrav = fallingGrav;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FallingGrav();
    }

    void FallingGrav()
    {
        if (!pm.isGrounded && !wallRun.wallRunning)
        {
            currentFallGrav = Mathf.Lerp(currentFallGrav, maxFallGrav, timeToMaxGrav * Time.deltaTime);
            rb.AddForce(Vector3.down * currentFallGrav, ForceMode.Force);
        }
        else
        {
            currentFallGrav = fallingGrav;
        }
    }
}
