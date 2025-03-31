using UnityEngine;

public class SpeedParticlesController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem speedParticles;
    public Rigidbody playerRb;

    [Header("Settings")]
    public float speedThreshold = 5f;
    public float maxEmissionRate = 150f;

    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (playerRb == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerRb = playerObj.GetComponent<Rigidbody>();
            }
            else
            {
                Debug.LogWarning("No GameObject with tag 'Player' found.");
            }
        }

        if (speedParticles != null)
        {
            emission = speedParticles.emission;
        }
        else
        {
            Debug.LogWarning("SpeedParticles not assigned.");
        }
    }

    void Update()
    {
        if (playerRb == null || speedParticles == null) return;

        float speed = playerRb.linearVelocity.magnitude;

        if (speed > speedThreshold)
        {
            if (!speedParticles.isPlaying)
                speedParticles.Play();

            float rate = Mathf.Lerp(0, maxEmissionRate, (speed - speedThreshold) / maxEmissionRate);
            emission.rateOverTime = rate;
        }
        else
        {
            if (speedParticles.isPlaying)
                speedParticles.Stop();
        }
    }
}
