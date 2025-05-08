using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerAudio pa;

    [SerializeField] Animator anim;
    [SerializeField] int attackRange = 3;
    [SerializeField] int damage;
    bool isAttacking;

    private void Start()
    {
        anim = GetComponent<Animator>();
        pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        Attack();
        anim.speed = anim.speed * GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().playerTimeSlowMultiplier;
    }

    

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            pa.AttackSound();
            isAttacking = true;
            anim.SetBool("IsAttacking", true);
        }

        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
            {
                isAttacking = false;
                anim.SetBool("IsAttacking", false);
            }
        }
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
        pa.HitSound();
        yield return new WaitForSeconds(0.09f);
        anim.enabled = true;
        StopCoroutine(Hitstop());
    }


}
