using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerMovement pm;
    [SerializeField] WallRun wr;

    [Header("Audio Clips")]
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip slideSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip wallJumpSound;
    [SerializeField] AudioClip attackSound;

    [Header("Timers")]
    [SerializeField] float walkAudioTimer;
    [SerializeField] float runAudioTimer;
    

    float currentWalkTime;
    float currentRunTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSound();
    }

    void UpdateSound()
    {
        if (pm.isGrounded && pm.currentVelocity > 0.5f && !pm.isSliding)
        {
            if (!pm.isSprinting)
            {
                audioSource.clip = moveSound;
                currentWalkTime += Time.deltaTime;
                if (currentWalkTime >= walkAudioTimer)
                {
                    audioSource.Play();
                    currentWalkTime = 0;
                }
            }
            else if (pm.isSprinting)
            {
                audioSource.clip = moveSound;
                currentRunTime += Time.deltaTime;
                if (currentRunTime >= runAudioTimer)
                {
                    audioSource.Play();
                    currentRunTime = 0;
                }
            }
        }
        
    }
}
