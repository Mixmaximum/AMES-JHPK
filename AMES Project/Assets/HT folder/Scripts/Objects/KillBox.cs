using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] PlayerHealth ph;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.tag == "Player Collider")
        {
            Debug.Log("Collided with player");
            ph.Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            ph.Die();
        }
    }
}
