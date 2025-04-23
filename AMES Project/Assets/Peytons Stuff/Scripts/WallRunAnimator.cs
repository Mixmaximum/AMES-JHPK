using UnityEngine;

public class WallRunAnimator : MonoBehaviour
{
    [Header("Drag the script that contains 'LeftWall' and 'RightWall'")]
    public MonoBehaviour wallRunScriptReference;

    [Header("Drag the Animator component")]
    public Animator animator;

    private System.Type wallRunType;
    private System.Reflection.PropertyInfo leftWallProperty;
    private System.Reflection.PropertyInfo rightWallProperty;

    void Start()
    {
        if (wallRunScriptReference == null || animator == null)
        {
            Debug.LogError("WallRunAnimator: Assign both the wallRunScriptReference and Animator in the inspector.");
            enabled = false;
            return;
        }

        wallRunType = wallRunScriptReference.GetType();
        leftWallProperty = wallRunType.GetProperty("LeftWall");
        rightWallProperty = wallRunType.GetProperty("RightWall");

        if (leftWallProperty == null || rightWallProperty == null)
        {
            //Debug.LogError("WallRunAnimator: Script must have public bools 'LeftWall' and 'RightWall'.");
            enabled = false;
        }
    }

    void Update()
    {
        bool leftWall = (bool)leftWallProperty.GetValue(wallRunScriptReference);
        bool rightWall = (bool)rightWallProperty.GetValue(wallRunScriptReference);

        bool isWallRunning = leftWall || rightWall;

        animator.SetBool("isWallRunning", isWallRunning);

        if (leftWall)
            animator.SetInteger("wallSide", -1);
        else if (rightWall)
            animator.SetInteger("wallSide", 1);
        else
            animator.SetInteger("wallSide", 0);
    }
}
