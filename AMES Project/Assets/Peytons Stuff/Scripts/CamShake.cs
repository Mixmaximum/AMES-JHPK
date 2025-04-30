using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Shake Settings")]
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float shakeDuration = 0f;
    [SerializeField] private bool infiniteShake = false;

    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (infiniteShake || shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = originalPosition + shakeOffset;

            if (!infiniteShake)
            {
                shakeDuration -= Time.deltaTime;
                if (shakeDuration <= 0f)
                {
                    StopShake();
                }
            }
        }
    }

    // Public methods to trigger shake manually
    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        infiniteShake = false;
    }

    public void StartInfiniteShake(float magnitude)
    {
        infiniteShake = true;
        shakeMagnitude = magnitude;
    }

    public void StopShake()
    {
        infiniteShake = false;
        shakeDuration = 0f;
        transform.localPosition = originalPosition;
    }
}
