using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JorogumoStateMachine : MonoBehaviour
{
    #region variables
    public Transform[] patrolPoints;
    Transform currentPatrol;
    public float idleDuration = 3;
    float idleTimer;

    private int currentPatrolIndex = 0;

    public float sightRange = 25;
    public float rangedRange = 15f; // Adjust the ranged attack range
    public float rangedCooldown = 5f; // Adjust the ranged attack cooldown
    bool isRangedCooledDown = false;
    public float spellRange = 10f; // Adjust the spell range
    public float channelTime = 2f; // Adjust the channeling time
    public float spellCooldown = 10f; // Adjust the spell cooldown
    bool isSpellCooledDown = false;

    public float fleeDuration = 3;
    public float fleeThreshold = 25;
    public float fleeDistance = 5;


    private Transform playerPosition;
    private NavMeshAgent agent;
    private Animator anim;
    //public ParticleSystem deathParticel, attackParticle, attack2Particle, attack3Particle;

    SpiderlingManager spiderlingManager;
    JorogumoAttacks joroAttacks; //getting attack script reference
    #endregion

    #region States
    /// Declare states. If you add a new state remember to add a new States enum for it. 
    /// These states are what we use to change the finiate state machine (FSM) coroutine between its different states.
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING, CAST, DEATH, FLEEING }

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
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        joroAttacks = GetComponent<JorogumoAttacks>();
        stats = GetComponent<Stats>();
        spiderlingManager = GetComponent<SpiderlingManager>();


        //start the fsm, it's never turned off. Initiates the changes between the corroutiens. 
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
            Debug.Log("Starting state =" + currentState);
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behaviour Coroutines-
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >

        Debug.Log("*spiders chittering*");
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsIdle", true);

        //UPDATE IDLE STATE >
        while (currentState == States.IDLE)
        {
            //check for player and count until idle time has run out
            idleTimer += Time.deltaTime;

            if (IsInRange(rangedRange)) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;

            if (idleTimer > idleDuration) currentState = States.PATROLLING;


            //run through above once, then wait
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        anim.SetBool("IsIdle", false);
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
            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
                yield break; // Exit the patrol loop and start chasing
            }

            if (isSpellCooledDown)
            {
                anim.SetTrigger("SpellCast");//run the attack animation
                joroAttacks.StartSpellCast();

                StartCoroutine(SpellCooldown());

            }

            // Continue patrolling
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; //makes patrol point next one on list, and loops if it exceeds array limit.
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            
           
            yield return new WaitForSeconds(UnityEngine.Random.Range(idleDuration - 1, idleDuration + 1));
            
        }

        //exit state 
        anim.SetBool("IsMoving", false);
        yield return StartCoroutine(currentState.ToString());

    }

    IEnumerator CHASING()
    {
        //ENTER THE  STATE >
        Debug.Log("Fresh food!");
        anim.SetBool("IsMoving", true);

        agent.updateRotation = true;
        agent.SetDestination(playerPosition.position);

        //UPDATE  STATE >
        while (currentState == States.CHASING)
        {
            if (IsInRange(rangedRange)) currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.IDLE;

            agent.SetDestination(playerPosition.position);


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
        anim.SetBool("IsIdle", true);
        agent.isStopped = true;
        agent.ResetPath();
        Debug.Log("Kill them!");
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        //UPDATE  STATE 
        while (currentState == States.ATTACKING)
        {
            spiderlingManager.SetSpiderlingsTarget(playerPosition.position);

            if (isSpellCooledDown)
            {
                anim.SetTrigger("SpellCast");//run the attack animation
                joroAttacks.StartSpellCast();

                StartCoroutine(SpellCooldown());

            }
            else if (IsInRange(rangedRange) && isRangedCooledDown)
            {
                anim.SetTrigger("RangedAttack");//run the attack animation
                joroAttacks.RangedAttack(playerPosition);

                StartCoroutine(RangedCooldown());

            }


            if (IsInRange(sightRange) && !IsInRange(rangedRange))
            {
                currentState = States.CHASING;
            }
            else if (!IsInRange(sightRange))
            {
                currentState = States.IDLE;
                Debug.Log("*confused chittering*");
            }



            yield return new WaitForEndOfFrame();
        }

        //EXIT  STATE
        //Debug.Log("Oh no I see the player
        anim.SetBool("IsIdle", false); //COULD INSTEASD BE A 'attacking' ANIMATION NOT IDLE
        yield return StartCoroutine(currentState.ToString());
    }

    IEnumerator FLEEING()
    {
        //ENTER THE  STATE >

        anim.SetBool("IsMoving", true);
        agent.updateRotation = true;
        Debug.Log("I'm too pretty to die!");



        //UPDATE  STATE >
        while (currentState == States.FLEEING)
        {
            // Calculate a destination opposite to the player's position
            Vector3 fleeDestination = transform.position + (transform.position - playerPosition.position).normalized * fleeDistance;
            agent.SetDestination(fleeDestination);

            if (stats.currentHealth > fleeThreshold) currentState = States.IDLE; 

            yield return new WaitForSeconds(fleeDuration);

        }

        //EXIT STATE
        anim.SetBool("IsMoving", false);

        yield return StartCoroutine(currentState.ToString());
        //Debug.Log("Oh no I see the player!");
    }
    #endregion


    public void DIE()
    {
        StopAllCoroutines();
        currentState = States.DEATH;
        anim.SetTrigger("Death"); //set trigger for death animation
        //Instantiate(deathParticle, transform.position, Quaternion.identity);
        stats.Die();

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



    bool IsInRange(float range)
    {
        if (Vector3.Distance(playerPosition.position, transform.position) < range)
            return true;
        else
            return false;
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

    

}

