using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro

public class Menu : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string teleportLocation = "SampleScene";
    [SerializeField] string teleport2 = "Win";

    [Header("Canvas Management")]
    public GameObject canvas;

    [Header("Fade Settings")]
    public Image fadeImage; // Fullscreen black Image
    public Image logoImage; // Logo Image
    public TMP_Text fadeText; // Text that fades with the logo
    public Image cubeLineImage; // Cube Line Image that fades with logo/text

    [Header("Fade Timings")]
    public float fadeToBlackDuration = 1f;
    public float logoFadeInDuration = 1f;
    public float logoDisplayDuration = 1f;
    public float logoFadeOutDuration = 1f;

    [Header("UI to Hide")]
    public GameObject[] uiElementsToHide; // UI elements to hide during fade

    private void Start()
    {
        if (canvas != null)
            canvas.GetComponent<Canvas>().enabled = false;
        GetComponent<Canvas>().enabled = true;

        // Start everything transparent
        if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);
        if (logoImage != null) logoImage.color = new Color(1, 1, 1, 0);
        if (fadeText != null) fadeText.color = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, 0);
        if (cubeLineImage != null) cubeLineImage.color = new Color(cubeLineImage.color.r, cubeLineImage.color.g, cubeLineImage.color.b, 0);
    }

    public void Beginning()
    {
        Time.timeScale = 1;

        // Hide all other UI
        foreach (GameObject uiElement in uiElementsToHide)
        {
            if (uiElement != null)
                uiElement.SetActive(false);
        }

        StartCoroutine(FadeSequence());
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
        if (canvas != null)
        {
            canvas.GetComponent<Canvas>().enabled = true;
            GetComponent<Canvas>().enabled = false;
        }
    }

    private IEnumerator FadeSequence()
    {
        // 1. Fade screen to black
        yield return StartCoroutine(FadeImage(fadeImage, 0, 1, fadeToBlackDuration));

        // 2. Fade in logo, text, and cube line together
        yield return StartCoroutine(FadeElements(0, 1, logoFadeInDuration));

        // 3. Wait with everything visible
        yield return new WaitForSeconds(logoDisplayDuration);

        // 4. Fade out logo, text, and cube line together
        yield return StartCoroutine(FadeElements(1, 0, logoFadeOutDuration));

        // 5. Switch to the next scene
        SceneManager.LoadScene(teleportLocation);
    }

    private IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = img.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            img.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        img.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    private IEnumerator FadeElements(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        Color logoColor = logoImage.color;
        Color textColor = fadeText.color;
        Color lineColor = cubeLineImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);

            if (logoImage != null)
                logoImage.color = new Color(logoColor.r, logoColor.g, logoColor.b, alpha);

            if (fadeText != null)
                fadeText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

            if (cubeLineImage != null)
                cubeLineImage.color = new Color(lineColor.r, lineColor.g, lineColor.b, alpha);

            yield return null;
        }

        if (logoImage != null)
            logoImage.color = new Color(logoColor.r, logoColor.g, logoColor.b, endAlpha);

        if (fadeText != null)
            fadeText.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);

        if (cubeLineImage != null)
            cubeLineImage.color = new Color(lineColor.r, lineColor.g, lineColor.b, endAlpha);
    }
}
