using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public Rigidbody rb; // Reference to the Rigidbody of the camera or its parent
    public float bobFrequencyIdle = 1.0f; // Frequency of the bobbing when idle
    public float bobFrequencyMoving = 2.5f; // Frequency of the bobbing when moving
    public float bobAmplitude = 0.1f; // Amplitude of the bobbing
    public float velocityThreshold = 0.1f; // Minimum velocity to start increasing bobbing speed

    private Vector3 initialPosition;
    private float bobbingOffset;
    private float currentBobFrequency;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("No Rigidbody found! Assign one to the CameraBobbing script.");
            }
        }

        initialPosition = transform.localPosition;
        currentBobFrequency = bobFrequencyIdle; // Start with idle bobbing
    }

    void Update()
    {
        if (rb == null) return;

        Vector3 velocity = rb.linearVelocity;
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        if (speed > velocityThreshold)
        {
            currentBobFrequency = bobFrequencyMoving;
        }
        else
        {
            currentBobFrequency = bobFrequencyIdle;
        }

        bobbingOffset += Time.deltaTime * currentBobFrequency;
        float bobbing = Mathf.Sin(bobbingOffset) * bobAmplitude;
        transform.localPosition = initialPosition + new Vector3(0, bobbing, 0);
    }
}
