using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spiderling : MonoBehaviour
{
    public float followRange = 4f;
    public bool isFollowing = true;
    public bool isAttacking = false;

    public float meleeRange = 3f;
    public float meleeCooldown = 2f;
    public float minMeleeDamage = 3f;
    public float maxMeleeDamage = 6f;
    bool isMeleeCooledDown = true;

    public StatSystem.DamageType meleeDamageType; //this will just be standard damage

    private SpiderlingManager broodmother; //the jorogumo this spider is following
    private Vector3 target; //what this spiderling is curently moving to or attacking. Set by Joro.
    public float spiderlingLeashDistance = 20f;
    private Stats playerStats;

    NavMeshAgent navAgent;
    Stats stats; 

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        stats = GetComponent<Stats>();


    }

    void Update()
    {
        if (isFollowing && broodmother != null)
        {
            float distanceToMaster = Vector3.Distance(transform.position, broodmother.transform.position);

            if (distanceToMaster > followRange)
            {
                // Calculate a random point within a radius around the master
                Vector3 randomDestination = RandomNavSphere(broodmother.transform.position, 3);

                // Set the destination to the random point
                navAgent.SetDestination(randomDestination);
            }

        }

        //checking if close enough to target to attack,  attack if you are move to it if you arent
        if(target != null )
        {
            isAttacking = true;
            isFollowing = false;

            float distanceToTarget = Vector3.Distance(transform.position, target);

            if (distanceToTarget <= meleeRange && isMeleeCooledDown && isAttacking)
            {
                // perform Melee attack
                navAgent.ResetPath();
                DamagePlayer();
                StartCoroutine(MeleeCooldown());
                Debug.Log("SpiderAttackCalled");
            }
            else if (distanceToTarget > meleeRange)
            {
                navAgent.SetDestination(target);

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

    
    
    //Sets a new target for this spiderling to move to
    public void SetMoveTarget(Vector3 targetPosition)
    {
        target = targetPosition;
        isAttacking = false;
        isFollowing = true;
        navAgent.SetDestination(target);

    }

    //Sets a new target for this spiderling to attack 
    public void SetAttackTarget(Transform attackTarget)
    {
        target = attackTarget.position;
        isAttacking = true;
        isFollowing = false;
        navAgent.SetDestination(target);
    }

    void DamagePlayer()
    {
        //play animations/effects/sounds

        if (playerStats != null)
        {
            // Apply damage to the player
            StatSystem.DealDamage(playerStats, meleeDamageType, Random.Range(minMeleeDamage, maxMeleeDamage));
            Debug.Log("SpiderDamageCalled");

        }
    }

    IEnumerator MeleeCooldown()
    {
        isMeleeCooledDown = false;
        yield return new WaitForSeconds(meleeCooldown);
        isMeleeCooledDown = true;

    }

    IEnumerator DEATH()
    {
        navAgent.SetDestination(transform.position);
        //anim.SetTrigger("Death");
        //Instantiate(deathParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f); //wait for the animation/particle to end

        Destroy(gameObject, 1);

        Debug.Log("Blegh");
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

    public void DIE()
    {
        StopAllCoroutines();
        navAgent.updateRotation = false;
        StartCoroutine(DEATH());

    }

}
