using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float movementMultiplier = 10f;
    [Space(5)]

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpkey = KeyCode.Space;
    [Space(5)]

    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;
    [Space(5)]

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [Space(5)]

    float horizontalMovement;
    float verticalMovement;
    bool isGrounded;
    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);
        

        MyInput();
        ControlDrag();

        if (Input.GetKeyDown(jumpkey) && isGrounded)
        {
            Jump();
        }
    }
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlDrag()
    {
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
        rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
    }
}
