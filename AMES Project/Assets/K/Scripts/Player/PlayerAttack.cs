using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    Animator anim;
    [SerializeField] int attackRange = 4;
    [SerializeField] int damage;
    float cooldown;

    private void Start()
    {
        anim = GetComponent<Animator>();
        cooldown = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldown <= 0)
            StartCoroutine(Attack());
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }

    

    public IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        anim.SetBool("IsAttacking", true);
        cooldown = 0.75f;
        yield return new WaitForSeconds(0.75f);
        anim.SetBool("IsAttacking", false);
        StopCoroutine(Attack());
    }

    public void AttackDetection()
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

    public IEnumerator Hitstop()
    {
        anim.enabled = false;
        Debug.Log("Hit");
        yield return new WaitForSeconds(0.09f);
        anim.enabled = true;
        Debug.Log("Stop!");
        StopCoroutine(Hitstop());
    }

}
