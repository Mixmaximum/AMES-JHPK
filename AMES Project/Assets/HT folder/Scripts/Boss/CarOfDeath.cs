using UnityEngine;

public class CarOfDeath : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] GameObject targetObject;
    Vector3 target;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = targetObject.transform.position;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().timeMultiplier);
        if (transform.position == target)
        {
            transform.position = startingPos;
        }
    }
    
}
