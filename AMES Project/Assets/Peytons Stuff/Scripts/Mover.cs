using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform targetObject;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float duration = 2f;

    [Header("Animation Settings")]
    public Animator animator; // Drag in the Animator
    public string animationTriggerName = "StartMoving";

    [Header("Timing Settings")]
    public float delayBeforeStart = 3f; // Set this in Inspector to trigger after a few seconds

    private float elapsedTime = 0f;
    private bool isMoving = false;
    private bool hasMoved = false;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object not assigned!");
            return;
        }

        targetObject.position = startPosition;

        // Automatically start moving after a delay
        Invoke(nameof(StartMoving), delayBeforeStart);
    }

    void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        targetObject.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t >= 1f)
        {
            isMoving = false;
            hasMoved = true;
        }
    }

    private void StartMoving()
    {
        if (!isMoving && !hasMoved)
        {
            elapsedTime = 0f;
            isMoving = true;

            if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
            {
                animator.SetTrigger(animationTriggerName);
            }

            Debug.Log("Auto-triggered elevator movement after delay.");
        }
    }
}
