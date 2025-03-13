using UnityEngine;

public class RaycastInteract : MonoBehaviour
{
    [SerializeField]
    int interactRange = 4;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out hit, interactRange))
        {
            if(hit.collider.gameObject.tag == "MaskPickup" && Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.gameObject.GetComponent<BaseMaskPickup>().OnPickup();
            }
            if(hit.collider.gameObject.tag == "Interactable" && Input.GetKeyDown(KeyCode.E))
            {
                // this is for when later in development we might have things like buttons or doors we want to interact with.
            }
        }
    }
}
