using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    Animator anim;
    [SerializeField] int attackRange = 4;
    [SerializeField] int damage;
    bool isAttacking;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartCoroutine(Attack());

        if (isAttacking)
            anim.speed = .005f;
        else anim.speed = 1;
        
    }

    

    public IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        isAttacking = true;
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        StopCoroutine(Attack());
    }

    public void AttackDetection()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            if (hit.transform.CompareTag("Enemy"))
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
