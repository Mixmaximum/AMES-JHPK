using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] WallRun wallrun;
    [SerializeField] Animator animator;  // First Animator
    [SerializeField] Animator secondAnimator;  // Second Animator
    [SerializeField] Animator thirdAnimator;
    [SerializeField] Transform groundCheck;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float movementMultiplier = 10f;
    [SerializeField] float airMovementMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;

    [Header("Jumping/Ground")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask ground;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [Header("Ceiling Settings")]
    [SerializeField] private float ceilingCheckRange;

    [Header("Crouch Settings")]
    [SerializeField] private Vector3 crouchHeight;
    [SerializeField] private float crouchMoveSpeed;
    [SerializeField] private float crouchSprintSpeed;
    [SerializeField] private float crouchFloorDetectDist;

    [Header("Slide Settings")]
    [SerializeField] float requiredSlideSpeed;
    [SerializeField] float slideForce;
    [SerializeField] float maxSlideForce;

    [Header("Slide Jump/Fall Settings")]
    [SerializeField] float slideJumpForce = 10f;
    [SerializeField] float slideFallDelay;
    [SerializeField] float slideCoyoteTime;

    [Header("Audio")]
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource slideSound; // New slide sound
    [SerializeField] private AudioSource jumpSound; // New jump sound

    public float currentVelocity;
    float horizontalMovement;
    float verticalMovement;
    float currentSlideSpeed;
    float currentSlideDelayTime;
    float currentSlideCoyoteTime;
    float dAngle;

    public bool isSliding;
    public bool isSprinting;
    public bool isGrounded;
    bool isCrouching;
    bool ableToCrouch;
    bool isJumping;
    bool slidingOnSlope;
    bool isUnderCeiling;

    int impulseCounter;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 slideDirection;

    RaycastHit slopeHit;
    RaycastHit ceilingHit;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (maxSlideForce < slideForce)
        {
            maxSlideForce = slideForce;
        }
        rb.sleepThreshold = 0;
    }

    private void Update()
    {
        if (!isSliding)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        }
        else if (isSliding)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance * 2, ground);
        }
        ableToCrouch = Physics.CheckSphere(groundCheck.position, crouchFloorDetectDist, ground);
        CeilingCheck();
        MyInput();
        StartSlide();
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (isJumping && isGrounded)
        {
            isJumping = false;
        }

        if (isSliding && Input.GetKeyDown(jumpKey) ||isSliding && Input.GetKeyUp(slideKey))
        {
            //animator.SetBool("IsJumping", true);

            // Play the jump sound
            if (jumpSound != null && !jumpSound.isPlaying)
                jumpSound.Play();
            if (Input.GetKeyDown(jumpKey) && isGrounded || Input.GetKeyDown(jumpKey) && currentSlideCoyoteTime <= 0) //checks groundedness
            {
                // has player jump forwards
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(transform.up * slideJumpForce, ForceMode.Impulse);
                isJumping = true;
            }
            StopSlide();
        }

        Crouch();

        currentVelocity = rb.linearVelocity.magnitude;

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        UpdateAnimations();

        // Walking, Running, Sliding, and Jump Sounds
        HandleMovementSounds();
    }

    private void FixedUpdate()
    {
        ControlDrag();
        ControlSpeed();
        MovePlayer();
        if (isSliding)
        {
            SlideMovement();
        }

    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope() && !isSliding)
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if (isGrounded && OnSlope() && !isSliding)
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if (!isGrounded && !isSliding)
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMovementMultiplier, ForceMode.Acceleration);
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            dAngle = angle;
            return slopeHit.normal != Vector3.up;
        }
        return false;
    }

    void Jump()
    {
        rb.WakeUp();
        if (!isGrounded)
        {
            return;
        }

        if (isGrounded) //checks groundedness
        {
            // has player jump forwards
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        isJumping = true;
        //animator.SetBool("IsJumping", true);

        // Play the jump sound
        if (jumpSound != null && !jumpSound.isPlaying)
            jumpSound.Play();
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded && !isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            isSprinting = true;
        }
        else if (isCrouching && !Input.GetKey(sprintKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, crouchMoveSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }
        else if (Input.GetKey(sprintKey) && isCrouching)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSprintSpeed, acceleration * Time.deltaTime);
            isSprinting = true;
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            isSprinting = false;
        }
    }

    void ControlDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    void Crouch()
    {
        if (Input.GetKey(crouchKey) && ableToCrouch && !isSliding || isUnderCeiling && ableToCrouch && !isSliding)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight.y, transform.localScale.z);
            isCrouching = true;
            if (impulseCounter < 1)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                impulseCounter = 1;
            }
        }
        else if (!isSliding)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isCrouching = false;
            impulseCounter = 0;
        }
    }

    void CeilingCheck()
    {
        if (isSliding || isCrouching)
        {

            if (Physics.Raycast(transform.position, Vector3.up, ceilingCheckRange))
            {
                isUnderCeiling = true;
            }
            else
            {
                isUnderCeiling = false;
            }
        }
        else
        {
            isUnderCeiling = false;
        }
    }

    void StartSlide()
    {
        if (Input.GetKeyDown(slideKey) && !isCrouching && currentVelocity >= requiredSlideSpeed && !isSliding && isGrounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight.y, transform.localScale.z);
            isSliding = true;
            currentSlideSpeed = slideForce;
            slideDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
            currentSlideDelayTime = slideFallDelay;
            currentSlideCoyoteTime = slideCoyoteTime;
            Debug.Log("fwoomp");
        }
    }

    void SlideMovement()
    {
        // Get the player's current velocity in the direction of wallRunDirection
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, orientation.forward);

        // Accelerate only if under the desired wall run speed in that direction
        if (forwardSpeed < maxSlideForce)
        {
            if (!isGrounded)
            {
                if (slideFallDelay >= 0)
                {
                    slideFallDelay -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("No ground, returning");
                    return;
                }
            }
            // Apply the movement force
            if (OnSlope())
            {
                rb.AddForce(slopeMoveDirection * currentSlideSpeed, ForceMode.Force);
            }
            else
            {
                rb.AddForce(slideDirection.normalized * currentSlideSpeed, ForceMode.Force);
                Debug.Log("Applying Force");
            }
        }
        currentSlideCoyoteTime -= Time.deltaTime;
    }

    void StopSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void UpdateAnimations()
    {
        // Update the main animator
        animator.SetFloat("Speed", currentVelocity);
        //animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsFalling", !isGrounded);
        animator.SetBool("IsSliding", isSliding);  // Update sliding animation in first animator

        // Update the second animator
        //secondAnimator.SetFloat("Speed", currentVelocity);
        //secondAnimator.SetBool("IsGrounded", isGrounded);
        //secondAnimator.SetBool("IsFalling", !isGrounded);
        secondAnimator.SetBool("IsSliding", isSliding);  // Update sliding animation in second animator

        // Prevent speed-up during attack animation
        if (!thirdAnimator.GetBool("IsAttacking"))
        {
            thirdAnimator.speed = Mathf.Clamp(currentVelocity / 5f, 0.5f, 2f);
            secondAnimator.speed = Mathf.Clamp(currentVelocity / 5f, 0.5f, 2f); // Make sure the second animator matches the same speed
        }
        else if (thirdAnimator.GetBool("IsAttacking")) // i added this because i'm insane
        {
            thirdAnimator.speed = 1f; // Keep attack animation at normal speed
            secondAnimator.speed = 1f; // Keep second animator at normal speed
        }
    }

    void HandleMovementSounds()
    {
        // Only play sounds when grounded and not sliding
        if (isGrounded && !isSliding)
        {
            // Walking Sound
            if (currentVelocity > 0 && currentVelocity < sprintSpeed) // Walking
            {
                if (!walkSound.isPlaying)
                    walkSound.Play();

                // Adjust the pitch based on speed (higher speed = higher pitch)
                walkSound.pitch = Mathf.Lerp(1f, 1.5f, currentVelocity / sprintSpeed);

                if (runSound.isPlaying)
                    runSound.Stop();
            }
            else if (currentVelocity >= sprintSpeed) // Running
            {
                if (!runSound.isPlaying)
                    runSound.Play();

                // Adjust the pitch based on speed (higher speed = higher pitch)
                runSound.pitch = Mathf.Lerp(1f, 1.5f, (currentVelocity - sprintSpeed) / sprintSpeed);

                if (walkSound.isPlaying)
                    walkSound.Stop();
            }
            else
            {
                if (walkSound.isPlaying)
                    walkSound.Stop();

                if (runSound.isPlaying)
                    runSound.Stop();
            }
        }
        else if (isSliding) // Play slide sound when sliding
        {
            if (!slideSound.isPlaying)
                slideSound.Play();

            // Adjust the pitch based on sliding speed
            slideSound.pitch = Mathf.Lerp(1f, 1.5f, currentVelocity / sprintSpeed);

            // Stop other sounds during slide
            if (walkSound.isPlaying)
                walkSound.Stop();

            if (runSound.isPlaying)
                runSound.Stop();
        }
        else
        {
            // Stop sounds if not grounded
            if (walkSound.isPlaying)
                walkSound.Stop();

            if (runSound.isPlaying)
                runSound.Stop();

            if (slideSound.isPlaying)
                slideSound.Stop();
        }
    }
}
