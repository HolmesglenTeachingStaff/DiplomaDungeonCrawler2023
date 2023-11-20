using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JorogumoStateMachine : MonoBehaviour
{
    #region variables
    private bool isPatrolling = true;
    public Transform[] patrolPoints;
    public Transform currentPatrol;
    public float idleTimeMin = 2f;
    public float idleTimeMax = 5f;
    private int currentPatrolIndex = 0;

    public float sightRange = 25;
    public float meleeRange = 5f; // Adjust the melee attack range
    public float meleeCooldown = 3f; // Adjust the melee attack cooldown
    bool isMeleeCooledDown = false;
    public float rangedRange = 15f; // Adjust the ranged attack range
    public float rangedCooldown = 5f; // Adjust the ranged attack cooldown
    bool isRangedCooledDown = false;
    public float spellRange = 10f; // Adjust the spell range
    public float channelTime = 2f; // Adjust the channeling time
    public float spellCooldown = 10f; // Adjust the spell cooldown
    bool isSpellCooledDown = false;

    public float timeBetweenAttacks;
    float lastAttack; //How long ago the last attack was. Used for timers.

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

        lastAttack = 0;

        //start the fsm, it's never turned off. Initiates the changes between the corroutiens. 
        StartCoroutine(EnemyFSM());

        StartCoroutine(CastHealingSpell());
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
        //put any code here that you want to run at the start of the behaviour. EG: turning on/off any bools to cause any initial animations/effects/sounds to play, that should only be played once.

        float timer = 0; //creat a number to count from to track if idle should transition;
        Debug.Log("*spiders chittering*");

        //UPDATE IDLE STATE >
        //put any code here you want to repeat in idle. This while loop repeats for as long as the state stays on idle. 
        while (currentState == States.IDLE)
        {
            //check for player and count until idle time has run out
            if (IsInRange(meleeRange)) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else timer += Time.deltaTime;
            if (timer > 5) currentState = States.PATROLLING;
            yield return new WaitForEndOfFrame();

            //check player distance
            if (Vector3.Distance(transform.position, playerPosition.position) < sightRange)
            {
                //change state if (in this case) the distance to player is less than sight range
                currentState = States.CHASING;
            }

            //run through above once, then wait for (in this case) 0.5 seconds before running it again.
            yield return new WaitForSeconds(0.5f);
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
        yield return null;

    }

    IEnumerator PATROLLING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        Debug.Log("*huffs*");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            if (IsInRange(meleeRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;

            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            

            // Wait for the specified interval before the next roam
            yield return new WaitForSeconds(UnityEngine.Random.Range(idleTimeMin, idleTimeMax));

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        Debug.Log("Fresh food!");

        agent.updateRotation = true;
        agent.SetDestination(playerPosition.position);

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            if (IsInRange(rangedRange)) /* && Time.time - lastAttack > timeBetweenAttacks) */ currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            else agent.SetDestination(playerPosition.position);
            yield return new WaitForEndOfFrame();


        }

        //EXIT IDLE STATE
        //write any code here you want to run when the state is left

        //Debug.Log("Oh no I see the player!");
    }


    IEnumerator ATTACKING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("Die!");
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if (distanceToPlayer <= meleeRange && isMeleeCooledDown)
        {
            jorogumoAttacks.PerformMeleeAttack(playerPosition);

            // Start cooldown
            StartCoroutine(MeleeCooldown());

        }



        if (distanceToPlayer > meleeRange && distanceToPlayer <= rangedRange && isRangedCooledDown)
        {
            jorogumoAttacks.PerformRangedAttack(playerPosition);

            StartCoroutine(RangedCooldown());


        }


        if (distanceToPlayer <= spellRange && isSpellCooledDown)
        {
            jorogumoAttacks.StartSpellCast(playerPosition);

            StartCoroutine(SpellCooldown());

        }




        //UPDATE Chasing STATE 
        //put any code here you want to repeat during the state being active
        while (currentState == States.ATTACKING)
        {


            if (Vector3.Distance(transform.position, playerPosition.position) > sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("*confused ghostly groans*");
            }


            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE
        //write any code here you want to run when the state is left

        //Debug.Log("Oh no I see the player!");
    }


    #endregion


    //used to see the "sight range" for debugging, to make sure its not the sight range causing issues
    bool IsInRange(float range)
    {
        if (Vector3.Distance(playerPosition.position, transform.position) < range)
            return true;
        else
            return false;
    }


    public void DIE()
    {
        currentState = States.DEATH;
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


    /// For a "cone" of vision: Still have a vision 'raduius', draw two lines using X number for angle 'draw a line -X degrees from where this is facing, and another at +X degrees'. 
    /// use those lines to 'cut out' a cone from your vision radius. Then when a target is detected in vision radius, just check if it's angle from the enemy is 
    /// larger than right degree away or less than left degrees away. 
    // THERE IS A SAMPLE ONE IN LAST YEARS DUNGEON CRAWLER. Think maybe in resources SESSION 7? Use it for Jorogumo. 
    

    IEnumerator CastHealingSpell()
    {
        while (true)
        {
            yield return new WaitForSeconds(spellCooldown);
            if (spiderlingManager != null)
            {
                // Choose a minion to heal (customize as needed)
                Spiderling minionToHeal = FindDamagedSpiderling();

                if (minionToHeal != null)
                {
                    
                    //cast healing spell targeting said spiderling
                }
            }
        }
    }

    Spiderling FindDamagedSpiderling()
    {
        List<Spiderling> spiderlingsList = spiderlingManager.GetSpiderlings();

        if (spiderlingsList.Count > 0)
        {
            // Find the spiderling with the lowest health
            Spiderling lowestHealthspiderling = spiderlingsList[0];

            foreach (var spiderling in spiderlingsList)
            {
                // Access the 'Stats' component (customize as needed)
                Stats spiderlingStats = spiderling.GetComponent<Stats>();

                if (spiderlingStats != null)
                {
                    // Compare health and update the lowestHealthspiderling if needed
                    if (spiderlingStats.currentHealth() < lowestHealthspiderling.GetComponent<Stats>().currentHealth())
                    {
                        lowestHealthspiderling = lowestHealthgSpiderling;
                    }
                }
            }

            return lowestHealthspiderling;
        }

        return null;
    }


    



    IEnumerator MeleeCooldown()
    {
        isMeleeCooledDown = false;
        yield return new WaitForSeconds(meleeCooldown);
        isMeleeCooledDown = true;
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

