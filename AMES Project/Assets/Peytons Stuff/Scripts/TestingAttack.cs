using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    [SerializeField] private PlayerAudio pa;

    [Header("Attack Settings")]
    [SerializeField] private int attackRange = 3;
    [SerializeField] private int damage = 10;

    private bool isAttacking = false;

    private void Start()
    {
        if (pa == null)
            pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            pa.AttackSound();
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
        }

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

    // ✅ This will now be called from the animation event
    public void AttackDetection()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            if (hit.transform.CompareTag("Enemy") && !hit.transform.GetComponent<BaseEnemy>().isDead)
            {
                StartCoroutine(Hitstop());
                hit.transform.GetComponent<BaseEnemy>().TakeDamage(damage);
                hit.transform.GetComponent<BaseEnemy>().Knockback();
                Debug.Log("You hit it!");
            }
        }
    }

    private IEnumerator Hitstop()
    {
        animator.enabled = false;
        yield return new WaitForSeconds(0.09f);
        animator.enabled = true;
    }
}
