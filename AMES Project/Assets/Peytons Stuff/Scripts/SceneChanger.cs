using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("Time in seconds before the scene changes.")]
    public float timeToChange = 10f;

    [Tooltip("Name of the scene to load after the time passes.")]
    public string sceneToLoad;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToChange)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
