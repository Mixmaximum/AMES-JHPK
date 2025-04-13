using UnityEngine;

public class CameraZoomOnSpeed3D : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRb;
    public Camera cam;

    [Header("Zoom Settings")]
    public float speedThreshold = 10f;
    public float normalFOV = 60f;
    public float zoomedOutFOV = 80f;
    public float zoomSpeed = 2f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        float currentSpeed = playerRb.linearVelocity.magnitude;
        float targetFOV = currentSpeed > speedThreshold ? zoomedOutFOV : normalFOV;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
