using UnityEngine;

public class DelayedDoorOpen : MonoBehaviour
{
    [Tooltip("Delay (in seconds) before the door opens.")]
    public float delay = 11f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No Animator component found on this GameObject.");
            return;
        }

        Invoke(nameof(OpenDoor), delay);
    }

    void OpenDoor()
    {
        animator.SetTrigger("Open");
    }
}
