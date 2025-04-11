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
    [SerializeField] int playerHealth = 50;
    [SerializeField] int maxHealth = 50;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;

    private float currentCooldown;
    private float hitstopTime = 0.3f;

    private bool isAttacking = false;
    private bool canCombo = false;
    private bool comboQueued = false;

    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if(playerHealth <= 0)
        {
            Debug.Log("Player is Dead");
        }
    }

    private IEnumerator Attack1()
    {
        isAttacking = true;
        currentCooldown = 0f;
        anim.SetBool("IsAttacking", true);
        anim.Play("Attack1");

        PlaySound(attackSound);
        AttackDetection();
        yield return new WaitForSeconds(attack1Duration - comboWindow);

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

        PlaySound(attackSound);
        AttackDetection();
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
                PlaySound(hitSound);
                Debug.Log("Player Attack!");
            }
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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
