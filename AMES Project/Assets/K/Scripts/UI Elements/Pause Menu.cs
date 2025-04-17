using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    

    Canvas pauseMenuCanvas; // reference to the pause menu canvas
    [SerializeField] Canvas Canvas2;

    void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false; // the pause menu shouldn't be showing on startup 
        Canvas2.GetComponent<Canvas>().enabled = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1) // these four lines are just "If the canvas is disabled enable it else disable it"
        {
            pauseMenuCanvas.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0)
        {
            pauseMenuCanvas.enabled = false;
        }
        PauseGame();
        //Debug.Log($"Time.timeScale == {Time.timeScale.ToString()}"); // Sanity check to make sure that the time is actually changing when it should.
    }

    private void PauseGame()
    {
        if (pauseMenuCanvas.enabled == true) // I'm trying something a little different than usual here, basically time is stopped when the canvas is enabled 
        {
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = false; // disables the script that allows the player to look around.
            //GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastInteract>().enabled = false; // disables the script that allows the player to interact w/ objects
            Cursor.lockState = CursorLockMode.None; // these two lines just mess with the cursor
            Cursor.visible = true;
        }
        else // if the canvas is disabled then time is always unpaused, we'll see how this works.
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = true;
            //GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastInteract>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // methods for the buttons

    public void UnpauseGame()
    {
        pauseMenuCanvas.enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLook>().enabled = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // We don't actually have a main menu scene yet, so this is just kind of there at the moment
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenCanvas2()
    {
        Canvas2.GetComponent<Canvas>().enabled = true;
    }
    public void CloseCanvas2()
    {
        Canvas2.GetComponent<Canvas>().enabled = false;
    }
}
