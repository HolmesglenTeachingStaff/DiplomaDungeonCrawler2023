using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DR_SkeletonStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;

    private Animator anim;
    public Collider weapon1, weapon2, weapon3;
    public ParticleSystem deathParticel, attackParticle, attack2Particle, attack3Particle;

    //patrolling
    public List<Transform> waypoints;
    public int waypointIndex;

    [HideInInspector]
    public Color sightColor;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING,DIE }

    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //start the fsm
        StartCoroutine(EnemyFSM());
        anim = GetComponentInChildren<Animator>();

        //turn hitboxes off by default
        weapon1.enabled = false;
        weapon2.enabled = false;
        weapon3.enabled = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Behaviour Coroutines
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour
        float timer = 0; //creat a number to count from to track if idle should transition;


        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            //check for player and count up untile idle time has run out
            if (IsInRange(meleeRange)) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else timer += Time.deltaTime;
            if (timer > 5) currentState = States.PATROLLING;
            yield return new WaitForSeconds(0.1f);
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
        yield return null;

    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        agent.updateRotation = true;
        agent.SetDestination(player.position);


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            if (IsInRange(meleeRange)) currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            else agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator PATROLLING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator ATTACKING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.ATTACKING)
        {

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator DEATH()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.DIE)
        {

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    #endregion

    #region functions
    bool IsInRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
            return true;
        else
            return false;
    }
    public void DIE()
    {
        currentState = States.DIE;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
    #endregion

}

