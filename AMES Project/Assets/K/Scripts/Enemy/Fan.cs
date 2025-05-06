using UnityEngine;

public class Fan : MonoBehaviour
{

    Animator anim;
    BoxCollider box;
    [SerializeField] int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Fan Spin");
        box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.speed = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().timeMultiplier;
        if (GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().timeMultiplier == 0.5f)
            box.enabled = false;
        else box.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
