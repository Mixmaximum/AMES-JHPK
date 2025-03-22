using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;         // Reference to your player
    [SerializeField] private Animator cameraAnimator;           // Reference to the camera's Animator

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = playerTransform.position;
    }

    private void Update()
    {
        // Calculate movement speed
        Vector3 delta = playerTransform.position - lastPosition;
        float speed = delta.magnitude / Time.deltaTime;

        // Update the "Run" float in the Animator
        cameraAnimator.SetFloat("Run", speed);

        lastPosition = playerTransform.position;
    }
}
