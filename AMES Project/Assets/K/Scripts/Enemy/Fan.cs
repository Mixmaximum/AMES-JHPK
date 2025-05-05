using UnityEngine;

public class Fan : MonoBehaviour
{

    [SerializeField] int integer;
    Animator anim;
    BoxCollider box;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        if (integer == 1)
            anim.Play("Fan Animator");
        else anim.Play("Fan Animator 2");
    }

    // Update is called once per frame
    void Update()
    {
        anim.speed = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().timeMultiplier;
        if (GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().timeMultiplier == 0.5f)
            GameObject.FindGameObjectWithTag("Barrier").GetComponent<BoxCollider>().enabled = false;
        else GameObject.FindGameObjectWithTag("Barrier").GetComponent<BoxCollider>().enabled = true;
    }
}
