using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotationFollow : MonoBehaviour
{
    [SerializeField] Transform cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetRotation = cam.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(transform.rotation.x, targetRotation.y, transform.rotation.z);
    }
}
