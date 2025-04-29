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
    [SerializeField] float slideAudioTimer;
    

    float currentWalkTime;
    float currentRunTime;
    float currentSlideTime;

    bool audioStartW;
    bool audioStartR;
    bool audioStartS;

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
        if (pm.isGrounded && pm.currentVelocity > 0.5f )
        {
            if (!pm.isSprinting && !pm.isSliding)
            {
                audioSource.clip = moveSound;
                currentWalkTime += Time.deltaTime;
                if (!audioStartW)
                {
                    audioSource.Play();
                    audioStartW = true;
                }
                else if (currentWalkTime >= walkAudioTimer)
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
            if (pm.isSliding)
            {
                audioSource.clip = slideSound;
                currentSlideTime += Time.deltaTime;
                if (currentSlideTime >= slideAudioTimer)
                {
                    audioSource.Play();
                    currentSlideTime = 0;
                    Debug.Log("slide");
                }
            }
        }
        else
        {
            audioSource.Stop();
        }
        
    }
}
