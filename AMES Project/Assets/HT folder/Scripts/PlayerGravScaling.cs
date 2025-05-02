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

    public float currentFallGrav;
    float timeElapsed;
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
        float t = timeElapsed / timeToMaxGrav;
        if (!pm.isGrounded && !wallRun.wallRunning)
        {
            if (timeElapsed < timeToMaxGrav)
            {
                currentFallGrav = Mathf.Lerp(currentFallGrav, maxFallGrav, t);
                rb.AddForce(Vector3.down * currentFallGrav, ForceMode.Force);
                timeElapsed += Time.deltaTime;
            }
        }
        else
        {
            currentFallGrav = fallingGrav;
            timeElapsed = 0;
        }
    }
}
