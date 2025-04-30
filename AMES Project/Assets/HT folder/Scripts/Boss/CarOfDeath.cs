using Unity.VisualScripting;
using UnityEngine;

public class CarOfDeath : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    Vector3 target;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Car Target").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target)
        {
            transform.position = startingPos;
        }
    }
    
}
