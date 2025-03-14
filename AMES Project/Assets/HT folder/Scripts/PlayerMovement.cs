using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] WallRun wallrun;
    [Space(5)]

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float movementMultiplier = 10f;
    [SerializeField] float airMovementMultiplier = 0.4f;
    [Space(5)]

    [Header("Sprinting")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;
    [Space(5)]

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpkey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [Space(5)]

    [Header("Jumping/ground")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [Space(5)]

    [Header("Drag/Gravity")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float fallingGrav;
    [Space(5)]

    float horizontalMovement;
    float verticalMovement;
    bool isGrounded;
    Rigidbody rb;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        // use a raycast to detect if player is on a slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //checks for ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        

        MyInput();
        ControlDrag();
        ControlSpeed();
        FallingGrav();

        //Jumps
        if (Input.GetKeyDown(jumpkey) && isGrounded)
        {
            Jump();
        }

        // adjusts slope move direction to be slightly upwards
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        //Gets move inputs
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void Jump()
    {
        if (isGrounded) //checks groundedness
        {
            // has player jump forwards
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ControlSpeed()
    {
        // changes movespeed between sprint and walk over time if the player is grounded
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        // changes drag amount if the player is grounded or not
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
             rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePLayer();
    }

    void MovePLayer()
    {
        // moves the player based on move direction and speed
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        // moves the player based on sloped move direction and speed
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        // moves the player slower while in the air
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMovementMultiplier, ForceMode.Acceleration);
        }
    }

    void FallingGrav()
    {
        if (!isGrounded && !wallrun.wallRunning)
        {
            rb.AddForce(Vector3.down * fallingGrav, ForceMode.Force);
        }
    }
}
