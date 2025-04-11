using UnityEngine;

public class SlideEffectPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem slideEffect; // Or replace with AudioSource if using sound
    [SerializeField] private string slideAnimationName = "Slide"; // Match your slide animation's name

    private bool isEffectPlaying;

    void Update()
    {
        if (IsSlideAnimationPlaying())
        {
            if (!isEffectPlaying)
            {
                PlayEffect();
                isEffectPlaying = true;
            }
        }
        else
        {
            if (isEffectPlaying)
            {
                StopEffect();
                isEffectPlaying = false;
            }
        }
    }

    private bool IsSlideAnimationPlaying()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(slideAnimationName);
    }

    private void PlayEffect()
    {
        if (slideEffect != null)
        {
            slideEffect.Play();
        }
    }

    private void StopEffect()
    {
        if (slideEffect != null)
        {
            slideEffect.Stop();
        }
    }
}
