using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    enemyAI myEnemy;
    float currentRotationTime = 0;
    bool playerInSight = false;

    // Rotation speed for alert state
    public float rotationSpeed = 30f;

    // When we call the constructor, we save
    // a reference to our enemy's AI
    public AlertState(enemyAI enemy)
    {
        myEnemy = enemy;
    }

    // Here goes all the functionality that we want
    // what the enemy does when he is in this
    // state.
    public void UpdateState()
    {
        myEnemy.myLight.color = Color.yellow;

        if (playerInSight)
        {
            // Rotate the enemy slowly towards the player
            Vector3 directionToPlayer = (myEnemy.playerPosition - myEnemy.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            myEnemy.transform.rotation = Quaternion.RotateTowards(myEnemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // If the enemy is now looking at the player, transition to attack state
            if (Quaternion.Angle(myEnemy.transform.rotation, targetRotation) < 0.1f)
            {
                GoToAttackState();
                return;
            }
        }
        else
        {
            // Rotate slowly without focusing on player
            myEnemy.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // If we have rotated for the specified time and player is not in sight, go back to patrol state
        if (!playerInSight && currentRotationTime > myEnemy.rotationTime)
        {
            currentRotationTime = 0;
            GoToPatrolState();
        }
        else
        {
            currentRotationTime += Time.deltaTime;
        }
    }

    // If the Player has hit us, transition to attack state
    public void Impact()
    {
        // Not transitioning to attack state on impact
    }

    public void GoToAlertState()
    {
        // Already in the alert state
    }

    public void GoToAttackState()
    {
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        myEnemy.navMeshAgent.isStopped = false;
        myEnemy.currentState = myEnemy.patrolState;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInSight = true;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerInSight = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // Player exited, reset rotation time and player in sight
            currentRotationTime = 0;
            playerInSight = false;
        }
    }
}
