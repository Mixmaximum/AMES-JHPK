using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNinja : BaseEnemy
{
    // Enemy properties
    public EnemyNinja()
    {
        enemyName = "Enemy Ninja";
        maxHealth = 150;
        health = maxHealth;
        speed = 5f;
        damage = 20;
        isDead = false;
    }

    // References
    NavMeshAgent agent;
    DataHandler dH;
    Animator anim;
    Vector3 destination;
    Vector3 lookDir;
    [SerializeField] float maxCooldown = 2.0f;
    float currentCooldown;
    Rigidbody rBody;
    [SerializeField] float bodyCleanup;
    [SerializeField] float enemyVisionRange;
    [SerializeField] float enemyAttackDetectionRange = 3.0f;

    // Multiple particle systems
    [SerializeField] private GameObject[] deathParticlePrefabs;  // Array to hold multiple particle system prefabs

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dH = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>();
        anim = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        currentCooldown = maxCooldown;
    }

    public override void Movement() // Handles movement towards the player
    {
        agent.speed = speed * dH.timeMultiplier;
        agent.destination.Normalize();

        anim.SetInteger("Walking", (int)agent.velocity.x); // checks if the enemy is moving
        if (agent.velocity.x == 0)
            anim.SetInteger("Walking", (int)agent.velocity.z);
        anim.speed = dH.timeMultiplier;

        if (!isDead && agent.enabled)
            transform.LookAt(lookDir, Vector3.up); // handles the enemy looking at the player
    }

    public override void Attack() // handles the enemy attacking if the player is within range
    {
        if (Vector3.Distance(destination, transform.position) <= 1.6f)
        {
            anim.SetBool("Close", true);
            if (currentCooldown >= maxCooldown)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(Stun());
                currentCooldown = 0f;
            }
        }
        else
            anim.SetBool("Close", false);
    }

    public void AttackDetection() // detects if the player is within range of an attack
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward);

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= enemyAttackDetectionRange && Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("The player was hit!");
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health <= 0)
            destination = transform.position;
    }

    public override void EnemyUpdate()
    {
        if (currentCooldown < maxCooldown)
            currentCooldown += Time.deltaTime;
        if (isDead)
            bodyCleanup += Time.deltaTime;
        if (bodyCleanup >= 30)
            Destroy(gameObject);
        PlayerDetection();
    }

    public void PlayerDetection() // detects if the player is within range of detection
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward);

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isSprinting && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 20f || Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.6f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
            lookDir = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            if (!isDead && agent.enabled)
                agent.destination = destination;
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isCrouching && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 2f || Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.6f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
            lookDir = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            if (!isDead && agent.enabled)
                agent.destination = destination;
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().moveSpeed != 0 && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 15f || Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.6f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
            lookDir = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            if (!isDead && agent.enabled)
                agent.destination = destination;
        }
    }

    private IEnumerator Stun()
    {
        agent.enabled = false;
        rBody.constraints = RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(0.325f);
        rBody.constraints = RigidbodyConstraints.None;
        if (!isDead)
            agent.enabled = true;
        StopCoroutine(Stun());
    }

    public override void Knockback()
    {
        rBody.AddForce(-transform.forward * -1000f, ForceMode.Impulse);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("I.. I am dead.");
        agent.enabled = false;
        anim.enabled = false;
        Knockback();
        anim.speed = 0;

        // Instantiate all death particle systems
        if (deathParticlePrefabs != null && deathParticlePrefabs.Length > 0)
        {
            foreach (var particlePrefab in deathParticlePrefabs)
            {
                if (particlePrefab != null)
                {
                    Instantiate(particlePrefab, transform.position, Quaternion.identity); // Spawn each particle system at the enemy's position
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerMovement>().isSliding)
        {
            OnDeath();
        }
    }
}
