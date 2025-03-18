using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : BaseEnemy
{
    public TestEnemy()
    {
        enemyName = "Test Enemy";
        maxHealth = 150;
        health = maxHealth;
        speed = 5f;
        damage = 20;
        isDead = false;
    }

    float maxCooldown = 5f;
    float currentCooldown;
    NavMeshAgent agent;

    public override void Movement() // Handles movement towards the player
    {
        agent.destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
        agent.destination.Normalize();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed; // speed obv set to the speed value
    }

    public override void Attack()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 2 && currentCooldown > maxCooldown)
        {
            currentCooldown = 0; // "Attacks" the player if they're within a certain distance and the cooldown is higher than the maxCooldown
            Debug.Log("Attack you!");
        }
    }

    public override void EnemyUpdate() // Handles the enemy attack cooldown
    {
        if (currentCooldown < maxCooldown)
            currentCooldown += Time.deltaTime;
    }
}
