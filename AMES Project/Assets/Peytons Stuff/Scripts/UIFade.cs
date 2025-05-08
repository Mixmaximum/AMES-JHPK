using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public Graphic graphicToFade;
    public float fadeDuration = 2f;
    public bool fadeInOnStart = true;
    public bool disableOnEscape = false; // ✅ Toggle in Inspector

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (graphicToFade != null)
        {
            // Start with 0 alpha (invisible)
            Color color = graphicToFade.color;
            color.a = 0f;
            graphicToFade.color = color;

            // Begin fade if enabled
            fadeCoroutine = StartCoroutine(FadeRoutine());
        }
    }

    private void Update()
    {
        if (disableOnEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            if (graphicToFade != null)
            {
                // Stop fade and hide graphic
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                Color color = graphicToFade.color;
                color.a = 0f;
                graphicToFade.color = color;
                graphicToFade.gameObject.SetActive(false); // ✅ Fully disable the GameObject
            }
        }
    }

    private IEnumerator FadeRoutine()
    {
        if (fadeInOnStart)
        {
            yield return StartCoroutine(FadeGraphic(0f, 1f, fadeDuration)); // Fade in
        }

        yield return StartCoroutine(FadeGraphic(1f, 0f, fadeDuration)); // Fade out
    }

    private IEnumerator FadeGraphic(float startAlpha, float endAlpha, float duration)
    {
        Color startColor = graphicToFade.color;
        Color endColor = startColor;
        startColor.a = startAlpha;
        endColor.a = endAlpha;

        float time = 0f;
        while (time < duration)
        {
            if (graphicToFade == null) yield break;

            graphicToFade.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if (graphicToFade != null)
            graphicToFade.color = endColor;
    }
}
