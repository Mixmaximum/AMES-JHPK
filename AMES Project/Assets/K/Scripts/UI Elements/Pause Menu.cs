using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    

    Canvas pauseMenuCanvas; // reference to the pause menu canvas

    void Start()
    {
        pauseMenuCanvas = GetComponent<Canvas>();
        pauseMenuCanvas.enabled = false; // the pause menu shouldn't be showing on startup 
    }

    
    void Update()
    {
        PauseGame();
        //Debug.Log($"Time.timeScale == {Time.timeScale.ToString()}"); // Sanity check to make sure that the time is actually changing when it should.
    }

    private void PauseGame()
    {
        if (pauseMenuCanvas.enabled == true) // I'm trying something a little different than usual here, basically time is stopped when the canvas is enabled 
            Time.timeScale = 0;
        else Time.timeScale = 1; // if the canvas is disabled then time is always unpaused, we'll see how this works.

        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuCanvas.enabled == false) // these four lines are just "If the canvas is disabled enable it else disable it"
            pauseMenuCanvas.enabled = true;
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuCanvas.enabled == true)
            pauseMenuCanvas.enabled = false;
    }

    // methods for the buttons

    public void UnpauseGame()
    {
        pauseMenuCanvas.enabled = false;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // We don't actually have a main menu scene yet, so this is just kind of there at the moment
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
