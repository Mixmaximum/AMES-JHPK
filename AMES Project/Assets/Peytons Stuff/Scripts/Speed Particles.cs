using UnityEngine;

public class SpeedParticlesController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem speedParticles;
    public Rigidbody playerRb;

    [Header("Settings")]
    public float speedThreshold = 5f;       // Speed before particles start
    public float maxSpeed = 15f;            // Speed where particles hit max
    public float maxEmissionRate = 150f;    // Max particles per second

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

            float t = Mathf.Clamp01((speed - speedThreshold) / (maxSpeed - speedThreshold));
            float rate = Mathf.Lerp(0f, maxEmissionRate, t);
            emission.rateOverTime = rate;
        }
        else
        {
            if (speedParticles.isPlaying)
                speedParticles.Stop();
        }
    }
}
