using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] WallRun wallrun;
    [SerializeField] Animator animator;
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

    [Header("Drag/Gravity")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float fallingGrav;

    [Header("Crouch Settings")]
    [SerializeField] private Vector3 crouchHeight;
    [SerializeField] private float crouchMoveSpeed;
    [SerializeField] private float crouchSprintSpeed;
    [SerializeField] private float crouchFloorDetectDist;

    [Header("Slide Settings")]
    [SerializeField] float requiredSlideSpeed;
    [SerializeField] float slideLength;
    [SerializeField] float slideForce;

    float horizontalMovement;
    float verticalMovement;
    float currentVelocity;
    float currentSlideSpeed;
    float slideTimer;

    bool isGrounded;
    bool isCrouching;
    bool isSliding;
    bool ableToCrouch;
    bool isJumping;

    int impulseCounter;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 slideDirection;

    RaycastHit slopeHit;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        ableToCrouch = Physics.CheckSphere(groundCheck.position, crouchFloorDetectDist, ground);

        MyInput();
        ControlDrag();
        ControlSpeed();
        FallingGrav();
        StartSlide();

        if (isSliding)
            SlideMovement();

        if (isSliding && Input.GetKeyUp(slideKey))
            StopSlide();

        Crouch();

        currentVelocity = rb.linearVelocity.magnitude;

        if (Input.GetKeyDown(jumpKey) && isGrounded)
            Jump();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
            return slopeHit.normal != Vector3.up;
        return false;
    }

    void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isJumping = true;
        animator.SetBool("IsJumping", true);
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded && !isCrouching)
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        else if (isCrouching && !Input.GetKey(sprintKey))
            moveSpeed = Mathf.Lerp(moveSpeed, crouchMoveSpeed, acceleration * Time.deltaTime);
        else if (Input.GetKey(sprintKey) && isCrouching)
            moveSpeed = Mathf.Lerp(moveSpeed, crouchSprintSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    void ControlDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    void FallingGrav()
    {
        if (!isGrounded && !wallrun.wallRunning)
            rb.AddForce(Vector3.down * fallingGrav, ForceMode.Force);
    }

    void Crouch()
    {
        if (Input.GetKey(crouchKey) && ableToCrouch && !isSliding)
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

    void StartSlide()
    {
        if (Input.GetKeyDown(slideKey) && !isCrouching && currentVelocity >= requiredSlideSpeed && !isSliding && isGrounded)
        {
            if (Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, ground))
            {
                slideDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
                transform.localScale = new Vector3(transform.localScale.x, crouchHeight.y, transform.localScale.z);
                isSliding = true;
                slideTimer = slideLength;
                currentSlideSpeed = slideForce;
            }
        }
    }

    void SlideMovement()
    {
        currentSlideSpeed = Mathf.Lerp(currentSlideSpeed, 0, slideTimer * Time.deltaTime);
        rb.AddForce(slideDirection.normalized * currentSlideSpeed, ForceMode.Force);

        if (Input.GetKeyDown(jumpKey) || Input.GetKeyUp(slideKey))
            StopSlide();
    }

    void StopSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void UpdateAnimations()
    {
        animator.SetFloat("Speed", currentVelocity);
        animator.speed = Mathf.Clamp(currentVelocity / 5f, 0.5f, 2f);

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsFalling", !isGrounded);

        // Reset jump animation when grounded again
        if (isJumping && isGrounded)
        {
            animator.SetBool("IsJumping", false);
            isJumping = false;
        }
    }
}
