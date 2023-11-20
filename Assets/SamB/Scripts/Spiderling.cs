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

    private SpiderlingManager broodMother; //the jorogumo this spider is following
    private Transform target; //what this spiderling is curently targeting. Could be set by the Jorogumo.

    void Update()
    {
        // Follow the jorogumo
        if (broodMother != null)
        {
            float distanceToMaster = Vector3.Distance(transform.position, broodMother.transform.position);
            if (distanceToMaster > followRange)
            {
                GetComponent<NavMeshAgent>().SetDestination(broodMother.transform.position);
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
                GetComponent<NavMeshAgent>().SetDestination(target.position);
            }
        }
    }

    //establishing reference to the SpiderlingManger that is controlling this spider
    public void SetBroodMother(SpiderlingManager broodMother)
    {
        this.broodMother = broodMother;
    }

    public SpiderlingManager GetBroodMother()
    {
        return broodMother;
    }

    //Sets a new target for this spiderling to attack
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void DamagePlayer()
    {
        //play animations/effects/sounds


        // Access the 'Stats' component 
        Stats playerStats = target.GetComponent<Stats>();

        if (playerStats != null)
        {
            // Apply damage to the player
            playerStats.TakeDamage(10); 
        }
    }

}
