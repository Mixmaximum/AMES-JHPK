using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Animator animator;
    private bool isAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
        }

        // Check if attack animation has ended
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
            {
                isAttacking = false;
                animator.SetBool("IsAttacking", false);
            }
        }
    }
}
