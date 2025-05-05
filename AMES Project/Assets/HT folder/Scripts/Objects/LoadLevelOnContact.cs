using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnContact : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] float timer;

    float currentTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player Collider")
        {
            currentTime = timer;
            TimerStart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            currentTime = timer;
            TimerStart();
        }
    }
    void TimerStart()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= timer)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
