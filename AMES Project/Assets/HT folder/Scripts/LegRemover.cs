using UnityEngine;

public class LegRemover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovement pm;
    [SerializeField] GameObject legs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!pm.isSliding)
        {
            legs.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else
        {
            legs.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
    }
}
