using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;

    Rigidbody rb;

    [SerializeField] float rbDrag = 6f;
    [SerializeField] float movementMultiplier = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    private void Update()
    {
        MyInput();
        ControlDrag();
    }
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    void ControlDrag()
    {
        rb.linearDamping = rbDrag;
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
