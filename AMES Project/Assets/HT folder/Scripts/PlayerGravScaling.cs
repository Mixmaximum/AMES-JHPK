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

    void Start()
    {
        if (maxFallGrav < fallingGrav)
        {
            maxFallGrav = fallingGrav;
        }
    }

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

                // Camera shake based on gravity intensity
                if (CameraShake.Instance != null)
                {
                    float shakeIntensity = Mathf.InverseLerp(fallingGrav, maxFallGrav, currentFallGrav);
                    float shakeMagnitude = 0.05f * shakeIntensity; // Adjust scale as needed
                    float shakeDuration = 0.1f;
                    CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);
                }

                timeElapsed += Time.deltaTime;
            }
        }
        else
        {
            currentFallGrav = fallingGrav;
            timeElapsed = 0;
        }
    }
    public void ResetGrav()
    {
        currentFallGrav = fallingGrav;
    }
}
