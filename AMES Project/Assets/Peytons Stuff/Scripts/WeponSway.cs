using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.1f; // Amount of sway
    public float swaySpeed = 5f;    // Speed of the sway effect

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position of the weapon
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Get mouse input (X and Y axis)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate sway based on mouse movement
        Vector3 sway = new Vector3(-mouseX, -mouseY, 0) * swayAmount;

        // Smoothly move the weapon to the new position
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + sway, Time.deltaTime * swaySpeed);
    }
}
