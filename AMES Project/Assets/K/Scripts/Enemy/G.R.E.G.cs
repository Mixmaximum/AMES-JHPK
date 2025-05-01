using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GREG : BaseEnemy
{
    public GREG()
    {
        enemyName = "G.R.E.G";
        maxHealth = _maxHealth;
        health = maxHealth;
        speed = 7f;
        damage = _damage;
        isDead = false;
    }

    NavMeshAgent agent;
    DataHandler dH;
    Animator anim;
    Vector3 destination;
    Vector3 lookDir;
    [SerializeField] float maxCooldown = 2.0f;
    float currentCooldown;
    Rigidbody rBody;
    [SerializeField] float enemyVisionRange = 50;
    [SerializeField] float enemyAttackDetectionRange = 3.0f;
    [SerializeField] Transform[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] int _maxHealth = 500;
    [SerializeField] int _damage = 50; // these values were chosen arbitrarily, feel free to change them i dont care.
    bool playerDetected;
    [SerializeField] float gregSpeedUpTime;
    [SerializeField] float gregAbilityCooldownCurrent;
    [SerializeField] float gregAbilityCooldown = 20f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dH = GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>();
        anim = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody>();
        currentCooldown = maxCooldown;

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];

        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public override void Movement() // Handles movement towards the player
    {
        agent.speed = speed * dH.timeMultiplier; // multiplying the speed by a variable that gets cut in half by the time slow mask
        agent.destination.Normalize();

        anim.SetInteger("Walking", (int)agent.velocity.x); // checks if the enemy is moving
        if (agent.velocity.x == 0)
            anim.SetInteger("Walking", (int)agent.velocity.z);
        anim.speed = dH.timeMultiplier;

        if (!isDead)
            transform.LookAt(lookDir, Vector3.up); // handles the enemy looking at the player 
    }

    public override void Attack() // handles the enemy attacking if the player is within range
    {
        if (Vector3.Distance(destination, transform.position) <= 1.6f && currentCooldown >= maxCooldown)
        {
            currentCooldown = 0f;
            anim.SetTrigger("Attack");
        }
    }

    public void AttackDetection() // detects if the player is within range of an attack and if the enemy is looking at the player
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward);

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= enemyAttackDetectionRange && Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("The player was hit!");
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().health <= 0) // enemys stop running towards you when you die
            destination = transform.position;
    }

    public override void EnemyUpdate()
    {
        if (currentCooldown < maxCooldown) // handles enemy attack cooldown
            currentCooldown += Time.deltaTime;
        if(!isDead)
        {
            PlayerDetection();
            ChangeDestination();
            GregRandomAbility();
        }
    }

    public void PlayerDetection() // detects if the player is within range of detection
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), transform.forward, Color.green, enemyVisionRange);

        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 15f || Physics.Raycast(ray, out hit, enemyVisionRange) && hit.collider.CompareTag("Player"))
        {
            playerDetected = true;
            destination = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 0.6f, GameObject.FindGameObjectWithTag("Player").transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z + 1.3f);
            lookDir = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            if (!isDead && agent.enabled)
                agent.destination = destination;
        }
        else playerDetected = false;
    }

    private IEnumerator Stun() // fudge factor
    {
        agent.enabled = false;
        rBody.constraints = RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(0.325f);
        rBody.constraints = RigidbodyConstraints.None;
        if (!isDead)
            agent.enabled = true;
        StopCoroutine(Stun());
    }

    public override void Knockback() // funny
    {
        rBody.AddForce(-transform.forward * -1000f, ForceMode.Impulse);
    }

    public override void OnDeath() // runs when the enemy dies
    {
        base.OnDeath();
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("I.. I am dead.");
        agent.enabled = false;
        anim.enabled = false;
        Knockback();
        anim.speed = 0;
    }

    void ChangeDestination() // extrapolated from hunters greg script
    {
        if (waypoints.Length == 0 || agent.pathPending) return;

        // Check if we've reached the destination
        if (agent.enabled && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Optionally check if the agent has stopped moving
            if (!agent.hasPath && !playerDetected && agent.enabled || agent.velocity.sqrMagnitude == 0f && !playerDetected && agent.enabled)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                lookDir = waypoints[currentWaypointIndex].position;
                agent.SetDestination(new Vector3(waypoints[currentWaypointIndex].transform.position.x + 1.5f, waypoints[currentWaypointIndex].transform.position.y + 1.5f, waypoints[currentWaypointIndex].transform.position.z + 1.5f));
            }
        }
    }

    void GregAbilities()
    {
        switch (RandNum())
        {
            case 1:
                GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().StartCoroutine(GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().GregSlowTime());
                return;
            case 2:
                StartCoroutine(GregSpeedUp());
                return;
            case 3:
                transform.position = destination;
                return;
        }
    }

    void GregRandomAbility()
    {
        if (gregAbilityCooldownCurrent <= 0 && playerDetected)
        {
            GregAbilities();
            gregAbilityCooldownCurrent = gregAbilityCooldown;
        }
        else if (playerDetected) gregAbilityCooldownCurrent -= Time.deltaTime;
    }

    IEnumerator GregSpeedUp()
    {
        speed = speed * 4;
        anim.speed = anim.speed * 4;
        yield return new WaitForSeconds(gregSpeedUpTime);
        speed = speed / 4;
        anim.speed = anim.speed / 4;
        StopCoroutine(GregSpeedUp());
    }

    int RandNum()
    {
        int randomInteger;
        randomInteger = Random.Range(1, 4);
        return randomInteger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && speed > 7f)
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
