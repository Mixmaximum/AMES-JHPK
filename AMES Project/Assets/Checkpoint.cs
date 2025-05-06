using UnityEngine;

public class Checkpoint : MonoBehaviour
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            ph.Die();
        }
    }
}
