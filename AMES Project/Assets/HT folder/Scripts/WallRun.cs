using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Wall Detection")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;
    [Space(5)]

    [Header("Wall Running")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallRunSpeed;
    [Space(5)]

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }

    // Wall run state
    bool wallLeft = false;
    bool wallRight = false;
    bool DirectionChosen = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;
    public bool wallRunning = false;
    private Vector3 wallRunDirection;

    // Expose the Animator to the Inspector
    [SerializeField] private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                PreWallRun();
                StartWallRun();
                Debug.Log("wall running on the left");
                anim.SetBool("LeftWall", true); // Set the LeftWall bool to true for animation
                anim.SetBool("RightWall", false); // Set RightWall bool to false
            }
            else if (wallRight)
            {
                PreWallRun();
                StartWallRun();
                Debug.Log("wall running on the right");
                anim.SetBool("RightWall", true); // Set the RightWall bool to true for animation
                anim.SetBool("LeftWall", false); // Set LeftWall bool to false
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
            StopWallRun();
            anim.SetBool("LeftWall", false);
            anim.SetBool("RightWall", false);
        }
    }

    bool CanWallRun()
    {
        //detect if the player is along a wall
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void CheckWall()
    {
        // Detect if a wall is on the left or right and push it out to left wall hit/ right wall hit accordingly
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    void PreWallRun()
    {
        if (!DirectionChosen)
        {
            wallRunDirection = orientation.forward; // set the forward force direction
            DirectionChosen = true;
        }
    }

    private void StartWallRun()
    {
        // Turn off grav while wall running
        rb.useGravity = false;

        // Force the player to go downwards slightly
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        rb.AddForce(wallRunDirection * wallRunSpeed, ForceMode.Force); // push player forward
        Debug.Log(wallRunDirection * wallRunSpeed);

        // Change fov to wall run fov over time
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        // set wall running bool to true to adjust gravity in player move script
        wallRunning = true;

        // if the wall is on the left the camera tilts negative to go away from the wall
        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }

        // if the wall is on the right the camera tilts positive
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                //add force in the left direction
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            if (wallRight)
            {
                //add force in the right direction
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    private void StopWallRun()
    {
        //Deactivate wall running bool
        wallRunning = false;
        // Reset grav to normal after wall run stops
        rb.useGravity = true;
        // Reset to normal fov
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        //Reset camera tilt
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
        //Reset forward direction
        DirectionChosen = false;
    }
}
