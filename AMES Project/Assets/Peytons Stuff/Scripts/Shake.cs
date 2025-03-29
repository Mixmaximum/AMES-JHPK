using UnityEngine;

public class WallRunCameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float baseShakeMagnitude = 0.1f;
    public float maxShakeMagnitude = 0.3f;
    public float shakeSpeed = 15f;
    public float maxSpeed = 20f; // Speed at which shake reaches max

    private Vector3 originalPos;
    private bool isShaking = false;
    private float currentShakeMagnitude = 0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            float x = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2f - 1f;
            float y = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2f - 1f;
            Vector3 shakeOffset = new Vector3(x, y, 0f) * currentShakeMagnitude;
            transform.localPosition = originalPos + shakeOffset;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 10f);
        }
    }

    public void StartShake(float speed)
    {
        isShaking = true;
        float t = Mathf.Clamp01(speed / maxSpeed);
        currentShakeMagnitude = Mathf.Lerp(baseShakeMagnitude, maxShakeMagnitude, t);
    }

    public void StopShake()
    {
        isShaking = false;
        currentShakeMagnitude = 0f;
    }
}
