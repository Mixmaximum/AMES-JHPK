using UnityEngine;

public class TimerCheck : MonoBehaviour
{
    GameObject timerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerManager = GameObject.FindGameObjectWithTag("timer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player Collider")
        {
            timerManager.GetComponent<Timer>().timerPaused = false;
            timerManager.GetComponent<Timer>().checkMet = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            timerManager.GetComponent<Timer>().timerPaused = false;
            timerManager.GetComponent<Timer>().checkMet = true;
        }
    }
}
