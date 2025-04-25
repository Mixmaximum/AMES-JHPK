using UnityEngine;
using UnityEngine.AI;

public class Greg : MonoBehaviour
{
    [SerializeField] string title = "Greg: Genetically Reprogrammed Enforcement Golem";
    [SerializeField] int health = 1000;
    [SerializeField] float speed;
    [SerializeField] float damage;

    [SerializeField] Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    public bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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

    // Update is called once per frame
    void Update()
    {
        ChangeDestination();
    }

    void ChangeDestination()
    {
        if (waypoints.Length == 0 || agent.pathPending) return;

        // Check if we've reached the destination
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Optionally check if the agent has stopped moving
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }
}
