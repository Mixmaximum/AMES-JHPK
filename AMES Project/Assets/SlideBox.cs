using UnityEngine;

public class SlideObjectActivator : MonoBehaviour
{
    [Header("References")]
    public Animator playerAnimator;
    public GameObject objectToActivate;

    [Header("Animator Bool Name")]
    public string slidingBoolName = "IsSliding";

    private bool wasSliding = false;

    void Update()
    {
        if (playerAnimator == null || objectToActivate == null)
            return;

        bool isSliding = playerAnimator.GetBool(slidingBoolName);

        // When sliding starts (true -> false transition)
        if (isSliding && !wasSliding)
        {
            objectToActivate.SetActive(true); // Turn on the object when sliding starts
        }
        // When sliding ends (false -> true transition)
        else if (!isSliding && wasSliding)
        {
            objectToActivate.SetActive(false); // Turn off the object when sliding ends
        }

        wasSliding = isSliding; // Store current state for next frame comparison
    }
}
