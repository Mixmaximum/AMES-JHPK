using UnityEngine;

public class FaceCameraDirection : MonoBehaviour
{
    public Transform cameraTransform;
    public float positionSmoothTime = 0.3f; // Smooth time for position
    public float rotationSmoothTime = 0.1f; // Smooth time for rotation
    private Vector3 currentVelocity;
    private float currentRotationVelocity;

    void Update()
    {
        if (cameraTransform == null)
            return;

        // Smoothly move the object to follow the camera's position (only X and Z)
        Vector3 targetPosition = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, positionSmoothTime);

        // Smoothly rotate the object to match the camera's Y rotation
        float targetRotation = cameraTransform.eulerAngles.y;
        float smoothedRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref currentRotationVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedRotation, 0f);
    }
}
