using UnityEngine;
using System.Collections;

public class FreezeAtStart : MonoBehaviour
{
    public MonoBehaviour playerMovementScript; // Drag your Player Movement script here
    public Rigidbody playerRigidbody;          // Drag your Player Rigidbody here (for 3D)
    public float freezeDuration = 3f;          // How long to freeze at start

    private RigidbodyConstraints originalConstraints;

    private void Start()
    {
        if (playerRigidbody != null)
            originalConstraints = playerRigidbody.constraints; // Save the original Rigidbody constraints

        StartCoroutine(FreezePlayer());
    }

    private IEnumerator FreezePlayer()
    {
        // Freeze player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Freeze player Rigidbody (no movement or physics)
        if (playerRigidbody != null)
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        yield return new WaitForSeconds(freezeDuration);

        // Unfreeze everything
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (playerRigidbody != null)
            playerRigidbody.constraints = originalConstraints; // Restore original Rigidbody constraints
    }
}
