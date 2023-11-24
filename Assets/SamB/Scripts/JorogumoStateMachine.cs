using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JorogumoStateMachine : MonoBehaviour
{
    #region variables
    public Transform[] patrolPoints;
    public Transform currentPatrol;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    private int currentPatrolIndex = 0;

    public float sightRange = 25;
    public float rangedRange = 15f; // Adjust the ranged attack range
    public float rangedCooldown = 5f; // Adjust the ranged attack cooldown
    bool isRangedCooledDown = false;
    public float spellRange = 10f; // Adjust the spell range
    public float channelTime = 2f; // Adjust the channeling time
    public float spellCooldown = 10f; // Adjust the spell cooldown
    bool isSpellCooledDown = false;

    private Transform playerPosition;
    private NavMeshAgent agent;
    private Animator anim;
    //public ParticleSystem deathParticel, attackParticle, attack2Particle, attack3Particle;

    SpiderlingManager spiderlingManager;
    JorogumoAttacks jorogumoAttacks; //getting attack script reference
    #endregion

    #region States
    /// Declare states. If you add a new state remember to add a new States enum for it. 
    /// These states are what we use to change the finiate state machine (FSM) coroutine between its different states.
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING, CAST, DEATH }

    //this variable holds ONE of the states, as it can only be one at once. Switches between them as this variable changes.
    public States currentState;
    #endregion

    Stats stats;

    #region Initialization
    //set default state
    private void Awake()
    {
        //starts in idle. It has conditions to switch to the others.
        currentState = States.IDLE;

    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spiderlingManager = GetComponent<SpiderlingManager>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        stats = GetComponent<Stats>();
    


        StartCoroutine(EnemyFSM());
    }
    #endregion

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        //while the coroutine is running (while im pretty sure it always)
        while (true)
        {
            //Check what STRING is specifically in the currentState Variable, then find a coroutine with that same name. thats why the variable is literally just holing a string of the enum name
            // and is why you must be sure TO NAME THE COROUTINES THE EXACT SAME AS THE ENUMS
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behaviour Coroutines-
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >

        float timer = 0; //creat a number to count from to track if idle should transition;
        Debug.Log("*spiders chittering*");
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsIdle", true);

        //UPDATE IDLE STATE >
        while (currentState == States.IDLE)
        {
            //check for player and count until idle time has run out
            if (IsInRange(rangedRange)) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else timer += Time.deltaTime;
            if (timer > 5) currentState = States.PATROLLING;

            //run through above once, then wait
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        yield return StartCoroutine(currentState.ToString());
        yield return null;

    }

    IEnumerator PATROLLING()
    {
        //ENTER THE STATE >
        anim.SetBool("IsMoving", true);
        Debug.Log("*huffs*");

        //UPDATE STATE >
        while (currentState == States.PATROLLING)
        {
            if (IsInRange(sightRange)) currentState = States.CHASING;

            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            

            // Wait for the specified interval before the next roam
            yield return new WaitForSeconds(UnityEngine.Random.Range(idleTimeMin, idleTimeMax));

            yield return new WaitForEndOfFrame();
        }

        //exit state 
        anim.SetBool("IsMoving", false);
        yield return StartCoroutine(currentState.ToString());

    }

    IEnumerator CHASING()
    {
        //ENTER THE  STATE >
        Debug.Log("Fresh food!");

        agent.updateRotation = true;
        agent.SetDestination(playerPosition.position);

        //UPDATE  STATE >
        while (currentState == States.CHASING)
        {
            if (IsInRange(rangedRange)) /* && Time.time - lastAttack > timeBetweenAttacks) */ currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            else agent.SetDestination(playerPosition.position);
            yield return new WaitForEndOfFrame();


        }

        //EXIT STATE
        anim.SetBool("IsMoving", false);

        yield return StartCoroutine(currentState.ToString());
        //Debug.Log("Oh no I see the player!");
    }


    IEnumerator ATTACKING()
    {
        //ENTER THE STATE >
        Debug.Log("Kill them!");
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);


        if (IsInRange(rangedRange) && isRangedCooledDown)
        {
            anim.SetTrigger("RangedAttack");//run the attack animation

            jorogumoAttacks.RangedAttack(playerPosition);
            StartCoroutine(RangedCooldown());

        }


        if (isSpellCooledDown)
        {
            jorogumoAttacks.StartCast(playerPosition);
            anim.SetTrigger("SpellCast");//run the attack animation
            StartCoroutine(SpellCooldown());

        }

        //UPDATE  STATE 
        while (currentState == States.ATTACKING)
        {


            if (!IsInRange(sightRange))
            {
                currentState = States.IDLE;
                Debug.Log("*confused chittering*");
            }


            yield return new WaitForEndOfFrame();
        }

        //EXIT  STATE
        anim.SetBool("IsMoving", false);
        //Debug.Log("Oh no I see the player
        yield return StartCoroutine(currentState.ToString());
    }


    #endregion


    bool IsInRange(float range)
    {
        if (Vector3.Distance(playerPosition.position, transform.position) < range)
            return true;
        else
            return false;
    }


    public void DIE()
    {
        anim.SetTrigger("SpellCast");//run the attack animation
        stats.Die();

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spellRange);

    }

    

    IEnumerator RangedCooldown()
    {
        isRangedCooledDown = false;
        yield return new WaitForSeconds(rangedCooldown);
        isRangedCooledDown = true;
    }


    IEnumerator SpellCooldown()
    {
        isSpellCooledDown = false;
        yield return new WaitForSeconds(spellCooldown);
        isSpellCooledDown = true;
    }

}

