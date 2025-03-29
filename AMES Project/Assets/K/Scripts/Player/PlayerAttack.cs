using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("Attack Settings")]
    [SerializeField] float interactRange;
    [SerializeField] int damage;
    [SerializeField] float cooldownMax = 0.8f;
    [SerializeField] float attack1Duration = 0.5f;
    [SerializeField] float comboWindow = 0.3f;

    private float currentCooldown;
    private float hitstopTime = 0.3f;

    private bool isAttacking = false;
    private bool canCombo = false;
    private bool comboQueued = false;

    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Cooldown();

        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking && currentCooldown >= cooldownMax)
            {
                StartCoroutine(Attack1());
            }
            else if (canCombo)
            {
                comboQueued = true;
            }
        }
    }

    private IEnumerator Attack1()
    {
        isAttacking = true;
        currentCooldown = 0f;
        anim.SetBool("IsAttacking", true);
        anim.Play("Attack1");

        yield return new WaitForSeconds(attack1Duration - comboWindow);

        // Open combo window
        canCombo = true;

        yield return new WaitForSeconds(comboWindow);

        if (comboQueued)
        {
            StartCoroutine(Attack2());
        }
        else
        {
            EndAttack();
        }

        canCombo = false;
        comboQueued = false;
    }

    private IEnumerator Attack2()
    {
        anim.Play("Attack2");

        // Optional: match this to your second animation length
        yield return new WaitForSeconds(attack1Duration);

        EndAttack();
    }

    private void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", false);
    }

    public void AttackDetection()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<BaseEnemy>().TakeDamage(damage);
                StartCoroutine(Hitstop());
            }
            Debug.Log("Player Attack!");
        }
    }

    private void Cooldown()
    {
        if (currentCooldown < cooldownMax)
            currentCooldown += Time.deltaTime;
    }

    private IEnumerator Hitstop()
    {
        anim.speed = 0;
        yield return new WaitForSeconds(hitstopTime);
        anim.speed = 1;
    }
}
