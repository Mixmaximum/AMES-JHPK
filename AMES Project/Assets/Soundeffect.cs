using UnityEngine;

public class TimedSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public bool playAfterDelay = true;
    public float delayTime = 2f;  // Time before playing the sound
    public float playDuration = 2f; // How long the sound should play

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource assigned.");
            return;
        }

        if (playAfterDelay)
        {
            Invoke(nameof(PlaySoundWithStop), delayTime);
        }
        else
        {
            PlaySound();
            Invoke(nameof(StopSound), playDuration);
        }
    }

    private void PlaySoundWithStop()
    {
        PlaySound();
        Invoke(nameof(StopSound), playDuration);
    }

    private void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
