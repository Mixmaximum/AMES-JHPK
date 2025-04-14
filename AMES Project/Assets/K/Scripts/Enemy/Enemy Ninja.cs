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

        if (Vector3.Distance(destination, transform.position) <= 2.0f)
        {
            anim.SetInteger("Walking", 0);
        }
    }

    public override void Attack()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 2 && currentCooldown > maxCooldown)
        {
            anim.SetTrigger("Attack");
        }
    }

    public override void EnemyUpdate() // Handles the enemy attack cooldown
    {
        if (currentCooldown < maxCooldown)
            currentCooldown += Time.deltaTime;
    }
}
