using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNinja : BaseEnemy
{
    public EnemyNinja()
    {
        enemyName = "Enemy Ninja";
        maxHealth = 150;
        health = maxHealth;
        speed = 5f;
        damage = 20;
        isDead = false;
    }

    NavMeshAgent agent;
    DataHandler dH;
    Animator anim;
    Vector3 destination;
    float maxCooldown = 5f;
    float currentCooldown;
    Rigidbody rBody;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dH = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>();
        anim = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
    }

    public override void Movement() // Handles movement towards the player
    {
        agent.speed = speed * dH.timeMultiplier; // multiplying the speed by a variable that gets cut in half by the time slow mask
        destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.6f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
        agent.destination = destination;
        agent.destination.Normalize();

        anim.SetInteger("Walking", (int)agent.velocity.x); // checks if the enemy is moving
        if (agent.velocity.x == 0)
            anim.SetInteger("Walking", (int)agent.velocity.z);
        anim.speed = dH.timeMultiplier;

        Vector3 LookDir = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);

        transform.LookAt(LookDir, Vector3.up); // handles the enemy looking at the player 
    }

    public override void Attack()
    {
        if (Vector3.Distance(destination, transform.position) <= 1.1f)
        {
            anim.SetBool("Close", true);
            if (currentCooldown > maxCooldown)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(Stun());
                currentCooldown = 0f;
            }
        }
        else anim.SetBool("Close", false);
    }

    public void AttackDetection()
    {
        if (Vector3.Distance(destination, transform.position) <= 1.1f) // if the player is close at a specific point in the animation, then they take damage.
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health <= 0) // enemys stop running towards you when you die
            destination = transform.position;
    }

    public override void EnemyUpdate() 
    {
        if (currentCooldown < maxCooldown) // handles enemy attack cooldown
            currentCooldown += Time.deltaTime;
    }

    private IEnumerator Stun() // fudge factor
    {
        speed = 0;
        yield return new WaitForSeconds(1.5f);
        speed = 5f;
        StopCoroutine(Stun());
    }

    public override void Knockback()
    {
        rBody.AddForce(transform.forward * -25, ForceMode.Impulse);
    }
}
