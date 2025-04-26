using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public Graphic graphicToFade;
    public float fadeDuration = 2f;
    public bool fadeInOnStart = true; // Only controls whether it fades in first

    private void Start()
    {
        if (graphicToFade != null)
        {
            StartCoroutine(FadeRoutine());
        }
    }

    private IEnumerator FadeRoutine()
    {
        if (fadeInOnStart)
        {
            yield return StartCoroutine(FadeGraphic(0f, 1f, fadeDuration)); // Fade in
        }

        yield return StartCoroutine(FadeGraphic(1f, 0f, fadeDuration)); // Always fade out
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
            graphicToFade.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        graphicToFade.color = endColor;
    }
}
