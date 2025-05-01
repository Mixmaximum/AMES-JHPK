using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerAudio pa;

    Animator anim;
    [SerializeField] int attackRange = 3;
    [SerializeField] int damage;
    float cooldown;
    [SerializeField] float maxCooldown = 1.465f;
    [SerializeField] Image attackCooldownImage;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cooldown = maxCooldown;
        pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldown >= maxCooldown)
            StartCoroutine(Attack());
        if (cooldown < maxCooldown)
            cooldown += Time.deltaTime;

        attackCooldownImage.fillAmount = cooldown / maxCooldown;

        if(cooldown >= maxCooldown)
        attackCooldownImage.enabled = false;
        else attackCooldownImage.enabled = true;

        anim.speed = anim.speed * GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().playerTimeSlowMultiplier;
        if (GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().playerTimeSlowMultiplier == 0.5f)
            maxCooldown = 3f;
        else maxCooldown = 1.465f;
    }

    

    public IEnumerator Attack() // plays the attack animation
    {
        pa.AttackSound();
        anim.SetBool("IsAttacking", true);
        cooldown = 0;
        yield return new WaitForSeconds(1f);
        anim.SetBool("IsAttacking", false);
        StopCoroutine(Attack());
    }

    public void AttackDetection() // detects if there is anything infront of the player when they attack
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, attackRange))
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

    public IEnumerator Hitstop() // hitstop for more oomph
    {
        anim.enabled = false;
        Debug.Log("Hit");
        yield return new WaitForSeconds(0.09f);
        anim.enabled = true;
        Debug.Log("Stop!");
        StopCoroutine(Hitstop());
    }

}
