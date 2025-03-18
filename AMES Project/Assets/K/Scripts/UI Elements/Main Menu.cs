using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Level One"); // Doesn't do anything, we aren't at that stage yet.
    }

    public void Settings()
    {
        Debug.Log("You opened the settings~!!");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
