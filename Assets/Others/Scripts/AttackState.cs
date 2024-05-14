using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    enemyAI myEnemy;
    float actualTimeBetweenShots = 0;

    // When we call the constructor, we save
    // a reference to our enemy's AI
    public AttackState(enemyAI enemy)
    {
        myEnemy = enemy;
    }

    // Here goes all the functionality that we want
    // what the enemy does when he is in this
    // state.
    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShots += Time.deltaTime;

        // If the player is out of the trigger, go back to alert state
        if (!myEnemy.playerTransform)
        {
            myEnemy.GoToAlertState();
        }
    }

    // If the Player has hit us, we do nothing
    public void Impact() { }

    // We are already in this state, so we never call it
    public void GoToAttackState() { }

    public void GoToPatrolState() { }

    public void GoToAlertState()
    {
        myEnemy.currentState = myEnemy.alertState;
    }

    // The player is already in our trigger
    public void OnTriggerEnter(Collider col) { }

    // We rotate the enemy to look at the Player while attacking
    public void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // We always look at the player
            Vector3 lookDirection = col.transform.position - myEnemy.transform.position;

            // We rotate around the Y axis
            myEnemy.transform.rotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0, lookDirection.z));

            // Shoot at the player
            if (actualTimeBetweenShots > myEnemy.timeBetweenShots)
            {
                actualTimeBetweenShots = 0;
                //col.gameObject.GetComponent<Shooter>().Hit(myEnemy.damageForce);
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        GoToAlertState();
    }
}