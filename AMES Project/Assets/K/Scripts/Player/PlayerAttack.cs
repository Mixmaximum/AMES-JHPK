using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    float interactRange;
    [SerializeField]
    int damage;
    [SerializeField]
    float cooldownMax = 0.8f;
    float currentCooldown;
    Animator anim;
    float hitstopTime = 0.3f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Cooldown();
        Attack();
    }

    public void AttackDetection() // this functions going to be called by the animation that plays when you attack, detects if an enemy is infront of you
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.gameObject.tag == "Enemy") // hitting an enemy makes them take damage and makes hitstop happen
            {
               hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
               StartCoroutine(Hitstop());
            }
            Debug.Log("Player Attack!");
        }
    }

    public void Attack() // Handles playing the attack animation
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentCooldown >= cooldownMax)
        {
            GetComponent<Animator>().Play("Attack");
            currentCooldown = 0f;
        }
    }

    private void Cooldown() // Handles attack cooldown, necessary to make sure you can't attack continously
    {
        if (currentCooldown < cooldownMax)
            currentCooldown += Time.deltaTime;
    }

    private IEnumerator Hitstop() // Makes attacks feel more impactful by stopping the animation on impact, like in fighting games
    {
        anim.speed = 0;
        yield return new WaitForSeconds(hitstopTime);
        anim.speed = 1;
        StopCoroutine(Hitstop());
    }
}
