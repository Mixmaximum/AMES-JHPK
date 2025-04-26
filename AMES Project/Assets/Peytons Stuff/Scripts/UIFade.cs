using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 2f;
    public bool fadeOutOnStart = true;

    private void Start()
    {
        if (fadeOutOnStart && imageToFade != null)
        {
            StartCoroutine(FadeImage(1f, 0f, fadeDuration));
        }
    }

    public void FadeIn()
    {
        if (imageToFade != null)
            StartCoroutine(FadeImage(0f, 1f, fadeDuration));
    }

    public void FadeOut()
    {
        if (imageToFade != null)
            StartCoroutine(FadeImage(1f, 0f, fadeDuration));
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        Color startColor = imageToFade.color;
        Color endColor = startColor;
        startColor.a = startAlpha;
        endColor.a = endAlpha;

        float time = 0f;
        while (time < duration)
        {
            imageToFade.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        imageToFade.color = endColor;
    }
}
