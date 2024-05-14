using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform playerTransform; // Assuming you set this somewhere, like in Start()
    [HideInInspector] public Vector3 playerPosition;


    public Light myLight;
    public float life = 100;
    public float timeBetweenShots = 1.0f;
    public float damageForce = 10;
    public float rotationTime = 3.0f;
    public float shotHeight = 0.5f;
    public Transform[] wayPoints;

    void Start()
    {
        // AI States.
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        // Start patrolling
        currentState = patrolState;

        // Keep a NavMesh reference
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set player transform reference, you may need to replace this with your actual player reference
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    void Update()
    {
        // Since our states don't inherit from
        // MonoBehaviour, its update is not called
        // automatically, and we'll take care of it
        // by calling it every frame.
        currentState.UpdateState();

        if (life <= 0) Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        life -= damage;
        currentState.Impact();
    }

    // Since our states don't inherit from
    // MonoBehaviour, we'll have to let them know
    // when something enters, stays, or leaves our
    // trigger.
    void OnTriggerEnter(Collider col)
    {
        currentState.OnTriggerEnter(col);
    }

    void OnTriggerStay(Collider col)
    {
        currentState.OnTriggerStay(col);
    }


    void OnTriggerExit(Collider col)
    {
        currentState.OnTriggerExit(col);
    }

    // Methods to transition to different states
    public void GoToPatrolState()
    {
        navMeshAgent.isStopped = false;
        currentState = patrolState;
    }

    public void GoToAlertState()
    {
        currentState = alertState;
    }

    public void GoToAttackState()
    {
        currentState = attackState;
    }
}
