using UnityEngine;

public class CameraSway : MonoBehaviour
{
    public float swayAmount = 2f;  // How much the object rotates
    public float smoothSpeed = 5f; // How quickly it moves back to center

    private Vector3 initialRotation;

    void Start()
    {
        initialRotation = transform.localEulerAngles; // Store the initial rotation
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate target rotation based on mouse movement
        Vector3 targetRotation = new Vector3(-mouseY * swayAmount, mouseX * swayAmount, 0);

        // Apply smooth rotation
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, initialRotation + targetRotation, Time.deltaTime * smoothSpeed);
    }
}
