using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player; // Reference to the player
    public float detectionRadius = 5f; // Radius to detect the player
    public Light spotlight; // Reference to the spotlight
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private bool playerDetected = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNextWaypoint();
    }

    void Update()
    {
        if (playerDetected)
        {
            LookAtPlayer();
        }
        else if (!playerDetected && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNextWaypoint();
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Player detected
            playerDetected = true;
            // Change spotlight color to yellow
            spotlight.color = Color.yellow;
            // Stop the agent
            agent.isStopped = true;
        }
        else
        {
            // Player not detected
            playerDetected = false;
            // Change spotlight color back to white
            spotlight.color = Color.white;
            // Resume patrol
            agent.isStopped = false;
        }
    }

    void SetNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned.");
            return;
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}





// using UnityEngine;
// using UnityEngine.AI;

// public class PatrolState : MonoBehaviour
// {
//     public Transform[] waypoints;
//     private int currentWaypointIndex = 0;
//     private NavMeshAgent agent;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         SetNextWaypoint();
//     }

//     void Update()
//     {
//         if (!agent.pathPending && agent.remainingDistance < 0.5f)
//         {
//             SetNextWaypoint();
//         }
//     }

//     void SetNextWaypoint()
//     {
//         if (waypoints.Length == 0)
//         {
//             Debug.LogWarning("No waypoints assigned.");
//             return;
//         }

//         currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//         agent.SetDestination(waypoints[currentWaypointIndex].position);
//     }
// }