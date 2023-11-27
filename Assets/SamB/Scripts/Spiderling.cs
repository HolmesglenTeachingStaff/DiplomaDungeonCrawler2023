using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spiderling : MonoBehaviour
{
    public float followRange = 5f;

    public float meleeRange = 2f;
    public float meleeCooldown = 2f;
    private float lastAttackTime; //tracks when the last attack was

    private SpiderlingManager broodmother; //the jorogumo this spider is following
    private Transform target; //what this spiderling is curently targeting. Could be set by the Jorogumo.
    public float spiderlingLeashDistance = 10f;

    NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        // Follow the jorogumo
        if (broodmother != null)
        {
            float distanceToMaster = Vector3.Distance(transform.position, broodmother.transform.position);

            if (distanceToMaster > followRange)
            {
                // Calculate a random point within a radius around the master
                Vector3 randomDestination = RandomNavSphere(broodmother.transform.position, followRange);

                // Set the destination to the random point
                navAgent.SetDestination(randomDestination);
            }
        }

        //set the player as the 'target' to go and attack
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= meleeRange && Time.time - lastAttackTime > meleeCooldown)
            {
                // perform Melee attack
                lastAttackTime = Time.time;
                DamagePlayer();
            }
            else
            {
                // Move towards target
                navAgent.SetDestination(target.position);
            }
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);
        return navHit.position;
    }

    //establishing reference to the SpiderlingManger that is controlling this spider
    public void SetBroodmother(SpiderlingManager broodMother)
    {
        this.broodmother = broodMother;
    }

    public SpiderlingManager GetBroodMother()
    {
        return broodmother;
    }

    //Sets a new target for this spiderling to attack
    public void SetTarget(Vector3 targetPosition)
    {
        if (Vector3.Distance(transform.position, broodmother.transform.position) > spiderlingLeashDistance)
        {
            SetTarget(broodmother.transform.position);
        }
        else
        {
            navAgent.SetDestination(targetPosition);

        }
        target = null; // Clear the target so it stops following the old target
    }

    void DamagePlayer()
    {
        //play animations/effects/sounds


        // Access the 'Stats' component 
        Stats playerStats = target.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            // Apply damage to the player
            playerStats.TakeDamage(10); 
        }
    }

}
