using UnityEngine;

public class SlideCameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public Animator playerAnimator;
    public string slidingAnimationName = "Slide"; // Use your actual animation state name
    public float shakeMagnitude = 0.2f;

    private Vector3 originalPos;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        originalPos = cameraTransform.localPosition;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(slidingAnimationName))
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
        }
        else
        {
            cameraTransform.localPosition = originalPos;
        }
    }
}
