using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] PlayerMovement pm;
    [SerializeField] WallRun wr;

    [Header("Audio Clips")]
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip runSound;
    [SerializeField] AudioClip slideSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip wallJumpSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hitSound;

    bool audioStopped;
    bool soundplayed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoundLoops();
    }

    void UpdateSoundLoops()
    {
        if (pm.isGrounded && pm.currentVelocity > 0.5f && !pm.isSliding && !wr.wallLeft && !wr.wallRight)
        {
            if (!pm.isSprinting && !pm.isSliding)
            {
                AudioClip clip = audioSource.clip;
                audioSource.clip = moveSound;
                if (clip != moveSound)//currentWalkTime >= walkAudioTimer)
                {
                    audioSource.Play();
                    //currentWalkTime = 0;
                }
                else if (audioStopped)
                {
                    audioSource.Play();
                    audioStopped = false;
                }
            }
            else if (pm.isSprinting)
            {
                AudioClip clip = audioSource.clip;
                audioSource.clip = runSound;
                if (clip != runSound) //currentRunTime >= runAudioTimer)
                {
                    audioSource.Play();
                    //currentRunTime = 0;
                }
                else if (audioStopped)
                {
                    audioSource.Play();
                    audioStopped = false;
                }
            }
        }
        else if (pm.isSliding)
        {
            AudioClip clip = audioSource.clip;
            Debug.Log("sSlide");
            audioSource.clip = slideSound; 
            if (clip != slideSound)//currentWalkTime >= walkAudioTimer)
            {
                audioSource.Play();
                //currentWalkTime = 0;
            }
            else if (audioStopped)
            {
                audioSource.Play();
                audioStopped = false;
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioStopped = true;
            Debug.Log("Stopping audio");
        }
        
    }
    public void WallJumpSound()
    {
        audioSource2.clip = wallJumpSound;
        audioSource2.Play();
    }
    public void JumpSound()
    {
        audioSource2.clip = jumpSound;
        audioSource2.Play();
    }
    public void AttackSound()
    {
        audioSource2.clip = attackSound;
        audioSource2.Play();
    }
    
    public void HitSound()
    {
        audioSource2.clip = hitSound;
        audioSource2.Play();
    }
}
