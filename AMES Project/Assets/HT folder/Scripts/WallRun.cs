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
    [SerializeField] float coyoteTime = 0.2f;
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
    [SerializeField] private WallRunCameraShake cameraShake;

    [Header("Audio")]
    [SerializeField] private AudioSource wallRunAudio;
    [SerializeField] private AudioSource wallJumpAudio;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private float basePitch = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (wallRunAudio != null)
        {
            wallRunAudio.loop = true;
            wallRunAudio.playOnAwake = false;
        }
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            coyoteTimer = coyoteTime;

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
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    WallJump();
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

        UpdateWallRunAudio();
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

        // Apply downward force to keep player pressed against the wall
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        // Get the player's current velocity in the direction of wallRunDirection
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, wallRunDirection);

        // Accelerate only if under the desired wall run speed in that direction
        if (forwardSpeed < wallRunSpeed)
        {
            rb.AddForce(wallRunDirection * wallRunSpeed, ForceMode.Force);
        }

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        cameraShake?.StartShake(rb.linearVelocity.magnitude);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            WallJump();
        }
    }

    private void StopWallRun()
    {
        wallRunning = false;
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
        DirectionChosen = false;

        cameraShake?.StopShake();

        // Stop sound
        if (wallRunAudio && wallRunAudio.isPlaying)
        {
            wallRunAudio.Stop();
        }
    }

    private void WallJump()
    {
        Vector3 jumpDirection = transform.up;

        if (wallLeft)
            jumpDirection += leftWallHit.normal;
        else if (wallRight)
            jumpDirection += rightWallHit.normal;

        // Preserve some horizontal momentum, but reset vertical speed
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(jumpDirection.normalized * wallRunJumpForce, ForceMode.Impulse);
  
        coyoteTimer = 0;

        // Play jump sound
        wallJumpAudio?.Play();
    }

    private void UpdateWallRunAudio()
    {
        if (wallRunAudio == null) return;

        bool isMoving = rb.linearVelocity.magnitude > 1f;

        if (wallRunning && isMoving)
        {
            if (!wallRunAudio.isPlaying)
                wallRunAudio.Play();

            float speedPercent = Mathf.Clamp01(rb.linearVelocity.magnitude / 10f);
            wallRunAudio.pitch = Mathf.Lerp(basePitch, maxPitch, speedPercent);
        }
        else if (wallRunAudio.isPlaying)
        {
            wallRunAudio.Stop();
        }
    }
}
