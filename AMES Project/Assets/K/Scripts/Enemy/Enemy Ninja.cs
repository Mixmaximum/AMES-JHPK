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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dH = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>();
        anim = GetComponent<Animator>();
    }

    public override void Movement() // Handles movement towards the player
    {
        agent.speed = speed * dH.timeMultiplier; // multiplying the speed by a variable that gets cut in half by the time slow mask
        destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 1.3f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
        agent.destination = destination;
        agent.destination.Normalize();

        anim.SetInteger("Walking", (int)agent.velocity.x);
        if (agent.velocity.x == 0)
            anim.SetInteger("Walking", (int)agent.velocity.z);
        if (agent.velocity.z == 0)
            anim.SetInteger("Walking", (int)agent.velocity.y);
    }

    public override void Attack()
    {
        if (Vector3.Distance(destination, transform.position) <= 1.5f)
        {
            if (currentCooldown > maxCooldown)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(Stun());
                currentCooldown = 0f;
            }
        }
    }

    public override void EnemyUpdate() // Handles the enemy attack cooldown
    {
        if (currentCooldown < maxCooldown)
            currentCooldown += Time.deltaTime;
    }

    public IEnumerator Stun()
    {
        speed = 0;
        yield return new WaitForSeconds(1);
        speed = 5f;
        StopCoroutine(Stun());
    }
}
