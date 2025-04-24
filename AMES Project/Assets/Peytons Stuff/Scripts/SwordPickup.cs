using UnityEngine;

public class EnableOnPlayerTouch3D : MonoBehaviour
{
    [SerializeField] private GameObject objectToEnable;

    void Start()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false); // Ensure it starts disabled
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Collider"))
        {
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
                Debug.Log("Player touched the object. Enabling target object.");
            }
        }
    }
}
