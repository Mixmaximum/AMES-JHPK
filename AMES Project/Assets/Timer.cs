using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float timer;
    [SerializeField] TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("Timer Time").GetComponent<TextMeshProUGUI>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("timer");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timerText == null)
        {
            timerText = GameObject.FindGameObjectWithTag("Timer Time").GetComponent<TextMeshProUGUI>();
        }
    }
}
