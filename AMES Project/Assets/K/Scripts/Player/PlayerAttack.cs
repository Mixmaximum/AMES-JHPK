using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    Animator anim;
    [SerializeField] int attackRange = 4;
    [SerializeField] int damage;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
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
