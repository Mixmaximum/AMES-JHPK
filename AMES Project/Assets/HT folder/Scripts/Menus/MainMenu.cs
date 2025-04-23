using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    string teleportLocation = "SampleScene";
    [SerializeField]
    string teleport2 = "Win";
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.GetComponent<Canvas>().enabled = false;
        GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Beginning()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(teleportLocation);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LevelLoad()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(teleport2);
    }
    public void Canvas2()
    {
        canvas.GetComponent<Canvas>().enabled = true;
        GetComponent<Canvas>().enabled = false;
    }
}
