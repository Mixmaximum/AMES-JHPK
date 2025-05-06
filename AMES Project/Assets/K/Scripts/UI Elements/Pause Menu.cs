using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Canvas pauseMenuCanvas;
    [SerializeField] Canvas Canvas2;
    [SerializeField] Canvas ControlCanvas;

    [SerializeField] private float delayBeforePause = 3f;  // Time (in seconds) before pause menu can be used
    private float timeSinceStart = 0f;  // Tracks time since the game started
    private bool canPause = false;  // Flag to allow pausing after the delay

    void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false;
        Canvas2.GetComponent<Canvas>().enabled = false;
        ControlCanvas.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        // Track time since the game started
        timeSinceStart += Time.unscaledDeltaTime;

        // Allow pausing after the specified delay
        if (timeSinceStart >= delayBeforePause)
        {
            canPause = true;
        }

        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1)
            {
                pauseMenuCanvas.enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0)
            {
                pauseMenuCanvas.enabled = false;
                Canvas2.GetComponent<Canvas>().enabled = false;
                ControlCanvas.GetComponent<Canvas>().enabled = false;
            }
        }

        PauseGame();
    }

    private void PauseGame()
    {
        if (pauseMenuCanvas.enabled == true || Canvas2.enabled == true || ControlCanvas.enabled == true)
        {
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UnpauseGame()
    {
        pauseMenuCanvas.enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCanvas2()
    {
        Canvas2.GetComponent<Canvas>().enabled = true;
        pauseMenuCanvas.enabled = false; // Hide pause menu when opening Canvas2
    }

    public void CloseCanvas2()
    {
        Canvas2.GetComponent<Canvas>().enabled = false;
        pauseMenuCanvas.enabled = true; // Re-enable pause menu when closing Canvas2
    }

    public void OpenControlCanvas()
    {
        ControlCanvas.GetComponent<Canvas>().enabled = true;
        pauseMenuCanvas.enabled = false; // Hide pause menu when opening Controls
    }

    public void CloseControlCanvas()
    {
        ControlCanvas.GetComponent<Canvas>().enabled = false;
        pauseMenuCanvas.enabled = true; // Re-enable pause menu when closing Controls
    }
}
