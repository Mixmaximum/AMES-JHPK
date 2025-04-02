using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Wall Detection")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallRunSpeed;

    [Header("Coyote Time")]
    [SerializeField] float coyoteTime = 0.2f; // Time buffer for jumping after leaving a wall
    private float coyoteTimer;

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }

    bool wallLeft = false;
    bool wallRight = false;
    bool DirectionChosen = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;
    public bool wallRunning = false;
    private Vector3 wallRunDirection;

    [SerializeField] private Animator anim;
    [SerializeField] private WallRunCameraShake cameraShake; // Reference to camera shake script

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            coyoteTimer = coyoteTime; // Reset coyote timer when touching a wall

            if (wallLeft)
            {
                PreWallRun();
                StartWallRun();
                anim.SetBool("LeftWall", true);
                anim.SetBool("RightWall", false);
            }
            else if (wallRight)
            {
                PreWallRun();
                StartWallRun();
                anim.SetBool("RightWall", true);
                anim.SetBool("LeftWall", false);
            }
            else
            {
                StopWallRun();
                anim.SetBool("LeftWall", false);
                anim.SetBool("RightWall", false);
            }
        }
        else
        {
            if (coyoteTimer > 0)
            {
                // Allow jumping for a short time after leaving the wall
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PerformWallJump();
                }
                coyoteTimer -= Time.deltaTime;
            }
            else
            {
                StopWallRun();
                anim.SetBool("LeftWall", false);
                anim.SetBool("RightWall", false);
            }
        }
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    void PreWallRun()
    {
        if (!DirectionChosen)
        {
            wallRunDirection = orientation.forward;
            DirectionChosen = true;
        }
    }

    private void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        rb.AddForce(wallRunDirection * wallRunSpeed, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);
        wallRunning = true;

        // Apply camera tilt
        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        // Start camera shake with speed-based intensity
        cameraShake?.StartShake(rb.linearVelocity.magnitude);

        // Jump off wall
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformWallJump();
        }
    }

    private void StopWallRun()
    {
        wallRunning = false;
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
        DirectionChosen = false;

        // Stop camera shake
        cameraShake?.StopShake();
    }

    private void PerformWallJump()
    {
        Vector3 jumpDirection = Vector3.zero;
        if (wallLeft)
            jumpDirection = transform.up + leftWallHit.normal;
        else if (wallRight)
            jumpDirection = transform.up + rightWallHit.normal;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
        rb.AddForce(jumpDirection * wallRunJumpForce * 100, ForceMode.Force);

        coyoteTimer = 0; // Reset coyote time after jumping
    }
}
