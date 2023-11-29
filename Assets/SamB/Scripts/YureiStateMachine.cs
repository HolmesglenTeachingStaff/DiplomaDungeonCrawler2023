using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YureiStateMachine : MonoBehaviour
{
    #region variables

    public bool isRoaming = true;
    public float roamRadius = 7f;
    public float roamInterval = 3f;

    //public bool isAttacking = false;

    public float sightRange = 25;
    public float meleeRange = 5f; // Adjust the melee attack range
    public float meleeCooldown = 3f; // Adjust the melee attack cooldown
    bool isMeleeCooledDown = true;
    public float rangedRange = 15f; // Adjust the ranged attack range
    public float rangedCooldown = 5f; // Adjust the ranged attack cooldown
    bool isRangedCooledDown = true;
    public float spellRange = 10f; // Adjust the spell range
    public float channelTime = 2f; // Adjust the channeling time
    public float spellCooldown = 10f; // Adjust the spell cooldown
    bool isSpellCooledDown = true;

    public float timeBetweenAttacks;

    private Transform playerPosition;
    private NavMeshAgent agent;
    private Animator anim;
    public ParticleSystem deathParticle;
    //public Transform currentPatrol;

    Stats stats;
    YureiAttacks yureiAttacks; //getting attack script reference

    #endregion

    #region States
    /// Declare states. If you add a new state remember to add a new States enum for it. 
    /// These states are what we use to change the finiate state machine (FSM) coroutine between its different states.
    public enum States { IDLE, ROAMING, CHASING, ATTACKING, CASTING, DEATH }

    //this variable holds ONE of the states, as it can only be one at once. Switches between them as this variable changes.
    public States currentState;
    #endregion

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
        yureiAttacks = GetComponent<YureiAttacks>();
        stats = GetComponent<Stats>();


        //start the fsm, it's never turned off. Initiates the changes between the corroutiens. 
        StartCoroutine(EnemyFSM());
    }
    #endregion

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        Debug.Log("Yurei FSM has started");

        while (true) //while the coroutine is running
        {
            Debug.Log("Starting state =" + currentState);
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behaviour Coroutines-
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsIdle", true);
        float idleTimer = 0; //idle timer
        Debug.Log("*Ghostly moans*");

        //UPDATE IDLE STATE >
        while (currentState == States.IDLE)
        {
            idleTimer += Time.deltaTime;
            Debug.Log("IDLE TIMER" + idleTimer);

            //check for player
            if (IsInRange(sightRange)) currentState = States.CHASING;

            if (idleTimer > 5) currentState = States.ROAMING;

            //start the corrountine of the current state if it's not idle
            //if (currentState != States.IDLE) StartCoroutine(currentState.ToString());

            //run through above once, then wait
            yield return new WaitForEndOfFrame();        
        }

        //EXIT IDLE STATE >
        StartCoroutine(currentState.ToString());
        yield return null;

    }

    IEnumerator ROAMING()
    {
        //ENTER THE roaming STATE
        Debug.Log("*Ghostly whooshes*");
        anim.SetBool("IsMoving", true); //making moving true
        float roamingTimer = 0; //idle timer


        //UPDATE roaming STATE >
        while (currentState == States.ROAMING)
        {
            roamingTimer += Time.deltaTime;

            if (IsInRange(sightRange)) currentState = States.CHASING;

            // Select a random destination within the roamRadius
            Vector3 randomDestination = transform.position + Random.insideUnitSphere * roamRadius;
            randomDestination.y = transform.position.y; // Keep the y-coordinate constant as we wdont want it moving up/down

            // Move towards the random destination
            agent.SetDestination(randomDestination);

            if (roamingTimer > 5) currentState = States.IDLE;

            //if (currentState != States.ROAMING) StartCoroutine(currentState.ToString());

            // Wait for the specified interval before the next roam
            yield return new WaitForSeconds(roamInterval); 
        }

        //EXIT roaming state
        StartCoroutine(currentState.ToString());
        anim.SetBool("IsMoving", false); 
        yield return null;

    }

    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        Debug.Log("A fresh Soul!");
        anim.SetBool("IsMoving", true); //making mvoing true


        agent.updateRotation = true;
        agent.SetDestination(playerPosition.position);

        //UPDATE Chasing STATE >
        while (currentState == States.CHASING)
        {
            if (IsInRange(meleeRange)) /*&& Time.time - lastAttack > timeBetweenAttacks) */ currentState = States.ATTACKING;
            if (IsInRange(rangedRange)) /* && Time.time - lastAttack > timeBetweenAttacks) */ currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.ROAMING;
            else agent.SetDestination(playerPosition.position);

            yield return new WaitForEndOfFrame();
        }

        //EXIT  STATE
        StartCoroutine(currentState.ToString());
        anim.SetBool("IsMoving", false); //making moving true
    }


    IEnumerator ATTACKING()
    {
        //ENTER THE STATE >
        anim.SetBool("IsIdle", true); 
        bool isCasting = yureiAttacks.isCasting;
        agent.isStopped = true;
        agent.ResetPath();
        Debug.Log("Die!");

        //UPDATE STATE 
        while (currentState == States.ATTACKING)
        {

            if (IsInRange(spellRange) && isSpellCooledDown && !isCasting)
            {
                anim.SetTrigger("SpellAttack"); //trigger the animation
                yureiAttacks.StartCast(playerPosition);

                // Start cooldown
                StartCoroutine(SpellCooldown());

            }
            else if (IsInRange(meleeRange) && isMeleeCooledDown && !isCasting)
            {
                anim.SetTrigger("MeleeAttack");
                yureiAttacks.MeleeAttack();

                StartCoroutine(MeleeCooldown());

            }
            else if (IsInRange(rangedRange) && isRangedCooledDown && !isCasting)
            {
                anim.SetTrigger("RangedAttack");
                yureiAttacks.RangedAttack(playerPosition);

                StartCoroutine(RangedCooldown());

            }
            


            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }
            else if (!IsInRange(sightRange) && !IsInRange(rangedRange))
            {
                currentState = States.IDLE;
                Debug.Log("*confused ghostly moans*");
            }




            yield return new WaitForSeconds(0.1f);
        }

        //EXIT STATE
        anim.SetBool("IsIdle", false);
        yield return StartCoroutine(currentState.ToString());
    }


    #endregion


    //used to check if something is "in range". basically just comparing two things.
    bool IsInRange(float range)
    {
        if (Vector3.Distance(transform.position, playerPosition.position) < range)
            return true;
        else
            return false;
    }


    public void DIE()
    {
        StopAllCoroutines();
        currentState = States.DEATH;
        anim.SetTrigger("Death"); //set trigger for death animation
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        stats.Die();

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spellRange);

    }

    IEnumerator MeleeCooldown()
    {
        isMeleeCooledDown = false;
        yield return new WaitForSeconds(meleeCooldown);
        isMeleeCooledDown = true;

        //isAttacking = false;

    }


    IEnumerator RangedCooldown()
    {
        isRangedCooledDown = false;
        yield return new WaitForSeconds(rangedCooldown);
        isRangedCooledDown = true;

        //isAttacking = false;

    }


    IEnumerator SpellCooldown()
    {

        isSpellCooledDown = false;
        yield return new WaitForSeconds(spellCooldown);
        isSpellCooledDown = true;

    }

}
