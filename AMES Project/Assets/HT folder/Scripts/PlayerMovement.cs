using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [Header("References")]
    [SerializeField] Transform orientation; // Used to determine movement direction based on camera/player rotation
    [SerializeField] WallRun wallrun; // Reference to the WallRun script (if applicable)
    [SerializeField] Animator animator; // Primary animator for player animations
    [SerializeField] Animator secondAnimator; // Additional animator
    [SerializeField] Animator thirdAnimator; // Third animator, possibly for combat or special animations
    [SerializeField] Transform groundCheck; // Transform used to check if the player is grounded

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f; // Movement speed
    [SerializeField] float movementMultiplier = 10f; // Ground movement force multiplier
    [SerializeField] float airMovementMultiplier = 0.4f; // Air movement force multiplier
    [SerializeField] float maxVelocity;

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f; // Walking speed
    [SerializeField] public float sprintSpeed = 6f; // Sprinting speed
    [SerializeField] float acceleration = 10f; // Rate of speed change

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;

    [Header("Jumping/Ground")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundDistance = 0.4f; // Distance to check for ground
    [SerializeField] LayerMask ground; // What is considered "ground"

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [Header("Ceiling Settings")]
    [SerializeField] private float ceilingCheckRange;

    [Header("Crouch Settings")]
    [SerializeField] private Vector3 crouchHeight; // Player height when crouching
    [SerializeField] private float crouchMoveSpeed;
    [SerializeField] private float crouchSprintSpeed;
    [SerializeField] private float crouchFloorDetectDist;

    [Header("Slide Settings")]
    [SerializeField] float requiredSlideSpeed;
    [SerializeField] float slideForce;
    [SerializeField] float maxSlideForce;
    [SerializeField] float minimumslideForce;

    [Header("Slide Jump/Fall Settings")]
    [SerializeField] float slideJumpForce = 10f;
    [SerializeField] float slideAirDrag;
    [SerializeField] float slideFallDelay;
    [SerializeField] float slideCoyoteTime;

    [Header("Audio")]
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource slideSound;
    [SerializeField] private AudioSource jumpSound;

    // Runtime variables
    public float currentVelocity;
    float horizontalMovement;
    float verticalMovement;
    float currentSlideSpeed;
    float currentSlideDelayTime;
    float currentSlideCoyoteTime;
    float dAngle;

    // State flags
    public bool isSliding;
    public bool isSprinting;
    public bool isGrounded;
    bool isCrouching;
    bool ableToCrouch;
    bool isJumping;
    bool slidingOnSlope;
    bool isUnderCeiling;

    int impulseCounter;

    // Movement direction vectors
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 slideDirection;

    // Slope and ceiling detection
    RaycastHit slopeHit;
    RaycastHit ceilingHit;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents player from tipping over

        // Ensure max slide force is at least as large as slide force
        if (maxSlideForce < slideForce)
        {
            maxSlideForce = slideForce;
        }

        // Prevent the Rigidbody from sleeping
        rb.sleepThreshold = 0;
    }

    private void Update()
    {
        // Update grounded check
        isGrounded = Physics.CheckSphere(groundCheck.position, isSliding ? groundDistance * 2 : groundDistance, ground);
        ableToCrouch = Physics.CheckSphere(groundCheck.position, crouchFloorDetectDist, ground);

        CeilingCheck(); // Check if player is under ceiling
        MyInput(); // Read player input
        StartSlide(); // Try initiating slide

        // Handle jumping
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (isJumping && isGrounded)
        {
            isJumping = false;
        }

        // Slide jump or slide end
        if ((isSliding && Input.GetKeyDown(jumpKey)) || (isSliding && Input.GetKeyUp(slideKey)))
        {
            if (jumpSound != null && !jumpSound.isPlaying)
                jumpSound.Play();

            if ((Input.GetKeyDown(jumpKey) && isGrounded) || Input.GetKeyDown(jumpKey) && currentSlideCoyoteTime <= 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(transform.up * slideJumpForce, ForceMode.Impulse);
                isJumping = true;
            }

            StopSlide(); // End slide
        }

        Crouch(); // Handle crouching logic
        currentVelocity = rb.linearVelocity.magnitude; // Update velocity
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal); // Adjust move direction on slopes
        UpdateAnimations(); // Update animation parameters
        HandleMovementSounds(); // Play appropriate movement sounds
    }

    private void FixedUpdate()
    {
        ControlDrag(); // Update drag based on grounded state
        ControlSpeed(); // Update speed based on input
        MovePlayer(); // Apply movement forces

        if (isSliding)
        {
            SlideMovement(); // Apply sliding forces
        }
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Calculate direction based on orientation
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
        // Check if player is on a slope
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

        if (!isGrounded) return;

        // Reset vertical velocity and apply upward force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        isJumping = true;

        if (jumpSound != null && !jumpSound.isPlaying)
            jumpSound.Play();
    }

    void ControlSpeed()
    {
        // Adjust moveSpeed based on sprint/crouch state
        if (Input.GetKey(sprintKey) && isGrounded && !isCrouching)
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        else if (isCrouching && !Input.GetKey(sprintKey))
            moveSpeed = Mathf.Lerp(moveSpeed, crouchMoveSpeed, acceleration * Time.deltaTime);
        else if (Input.GetKey(sprintKey) && isCrouching)
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSprintSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);

        isSprinting = Input.GetKey(sprintKey);
    }

    void ControlDrag()
    {
        // Update drag based on grounded state
        if (isSliding && !isGrounded)
        {
            rb.linearDamping = slideAirDrag;
        }
        else if (!isGrounded && !isSliding)
        {
            rb.linearDamping = airDrag;
        }
        else
        {
            rb.linearDamping = groundDrag;
        }
        
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
        // Check if there is a ceiling above the player when crouching or sliding
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
        // Start sliding if speed threshold is met and player is grounded and not crouching already
        if (Input.GetKeyDown(slideKey) && !isCrouching && currentVelocity >= requiredSlideSpeed && !isSliding && isGrounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight.y, transform.localScale.z);
            isSliding = true;
            if ()
            currentSlideSpeed = slideForce;

            // Set the slide direction based on player input
            slideDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

            // Set timers for coyote time and fall delay
            currentSlideDelayTime = slideFallDelay;
            currentSlideCoyoteTime = slideCoyoteTime;

            Debug.Log("fwoomp");
        }
    }

    void SlideMovement()
    {
        // Determine how fast player is moving in the forward direction
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, orientation.forward);

        // Only apply slide force if under the max slide speed
        if (forwardSpeed < maxSlideForce)
        {
            // Wait for fall delay if not grounded
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

            // Apply slide force either on a slope or flat surface
            if (OnSlope())
            {
                rb.AddForce(slopeMoveDirection * currentSlideSpeed, ForceMode.Force);
                Debug.Log("Applying Force (Slope)");
            }
            else
            {
                rb.AddForce(slideDirection.normalized * currentSlideSpeed, ForceMode.Force);
                Debug.Log("Applying Force (Flat)");
            }
        }

        // Decrease coyote time over time
        currentSlideCoyoteTime -= Time.deltaTime;
    }

    void StopSlide()
    {
        // Reset slide state and scale
        isSliding = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void UpdateAnimations()
    {
        // Update first animator with current movement state
        animator.SetFloat("Speed", currentVelocity);
        animator.SetBool("IsFalling", !isGrounded);
        animator.SetBool("IsSliding", isSliding);

        // Update second animator similarly (grounded and falling updates are commented)
        secondAnimator.SetBool("IsSliding", isSliding);

        // Control animation playback speed based on velocity unless attacking
        if (!thirdAnimator.GetBool("IsAttacking"))
        {
            thirdAnimator.speed = Mathf.Clamp(currentVelocity / 5f, 0.5f, 2f);
            secondAnimator.speed = Mathf.Clamp(currentVelocity / 5f, 0.5f, 2f);
        }
        else if (thirdAnimator.GetBool("IsAttacking"))
        {
            thirdAnimator.speed = 1f;
            secondAnimator.speed = 1f;
        }
    }

    void HandleMovementSounds()
    {
        // Only play footstep sounds when grounded and not sliding
        if (isGrounded && !isSliding)
        {
            if (currentVelocity > 0 && currentVelocity < sprintSpeed)
            {
                // Walking
                if (!walkSound.isPlaying)
                    walkSound.Play();

                walkSound.pitch = Mathf.Lerp(1f, 1.5f, currentVelocity / sprintSpeed);

                if (runSound.isPlaying)
                    runSound.Stop();
            }
            else if (currentVelocity >= sprintSpeed)
            {
                // Running
                if (!runSound.isPlaying)
                    runSound.Play();

                runSound.pitch = Mathf.Lerp(1f, 1.5f, (currentVelocity - sprintSpeed) / sprintSpeed);

                if (walkSound.isPlaying)
                    walkSound.Stop();
            }
            else
            {
                // Idle
                if (walkSound.isPlaying)
                    walkSound.Stop();

                if (runSound.isPlaying)
                    runSound.Stop();
            }
        }
        else if (isSliding)
        {
            // Sliding sound logic
            if (!slideSound.isPlaying)
                slideSound.Play();

            slideSound.pitch = Mathf.Lerp(1f, 1.5f, currentVelocity / sprintSpeed);

            if (walkSound.isPlaying)
                walkSound.Stop();
            if (runSound.isPlaying)
                runSound.Stop();
        }
        else
        {
            // Player is in air or not moving
            if (walkSound.isPlaying)
                walkSound.Stop();
            if (runSound.isPlaying)
                runSound.Stop();
            if (slideSound.isPlaying)
                slideSound.Stop();
        }
    }
}