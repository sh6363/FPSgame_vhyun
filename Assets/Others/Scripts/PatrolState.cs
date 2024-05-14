using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    enemyAI myEnemy;
    private int nextWayPoint = 0;

    // When we call the constructor, we save
    // a reference to our enemy's AI
    public PatrolState(enemyAI enemy)
    {
        myEnemy = enemy;
    }

    // Here goes all the functionality that we want
    // what the enemy does when he is in this
    // state.
    public void UpdateState()
    {
        myEnemy.myLight.color = Color.green;

        myEnemy.navMeshAgent.destination = myEnemy.wayPoints[nextWayPoint].position;

        if (myEnemy.navMeshAgent.remainingDistance <= myEnemy.navMeshAgent.stoppingDistance)
        {
            nextWayPoint = (nextWayPoint + 1) % myEnemy.wayPoints.Length;
        }

        //Debug.Log("navmesh is stopped? " + myEnemy.navMeshAgent.isStopped );
    }

    public void Impact()
    {
        myEnemy.GoToAlertState();
    }

    public void GoToAlertState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {
        //no need here
    }

    public void GoToPatrolState()
    {
        // Already in the patrol state.
    }

    // In this state, the player is already inside, so we will ignore it.
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GoToAlertState();
        }
    }

    public void OnTriggerStay(Collider col)
    {
        //no need here
    }

    // If the player is outside the enemy radius, the enemy changes to Alert State.
    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            myEnemy.GoToPatrolState();
        }
    }
}