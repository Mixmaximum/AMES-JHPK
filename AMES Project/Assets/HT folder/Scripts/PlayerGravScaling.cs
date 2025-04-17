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
        float t = Time.deltaTime / timeToMaxGrav;
        if (!pm.isGrounded && !wallRun.wallRunning)
        {
            currentFallGrav = Mathf.Lerp(currentFallGrav, maxFallGrav, t);
            rb.AddForce(Vector3.down * currentFallGrav, ForceMode.Force);
            Debug.Log(currentFallGrav);
        }
        else
        {
            currentFallGrav = fallingGrav;
        }
    }
}
