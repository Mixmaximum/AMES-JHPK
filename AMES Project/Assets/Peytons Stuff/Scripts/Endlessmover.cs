using UnityEngine;

public class EndlessVerticalMover : MonoBehaviour
{
    public float speed = 2f;           // How fast it moves downward
    public float repeatDistance = 10f; // How far down before it resets

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // If moved too far down, reset position to create seamless loop
        if (Vector3.Distance(startPosition, transform.position) >= repeatDistance)
        {
            transform.position = startPosition;
        }
    }
}
