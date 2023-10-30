using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// State machine for a Summoner, used to opperate and switch between states.
/// Will be used for the majority of managing a Summoners movements/actions.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BW_SummonerStateMachine : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    private Animator anim;

    public Transform player;
    public Transform summonLocation;

    public GameObject objectToSummon;

    public bool checkingForPlayer = true;

    [Header("Reaction Range Values")]
    public float sightRange;
    public float summonRange;
    public float dangerRange;
    public float meleeRange;
    public float bufferRange;

    //Nodes to indicate where to patrol.
    [SerializeField] Transform[] nodes;
    int currentNode;
    #endregion

    //The behaviour states available for a Summoner to switch between.
    #region States
    public enum States {IDLE, PATROLLING, MELEE, CHASING, COMBAT}

    public States currentState;
    #endregion

    //Starting all required components.
    #region Initialization
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(SummonerFSM());
    }
    #endregion

    //Initialize the state machine.
    #region Finite State Machine
    IEnumerator SummonerFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Update
    void Update()
    {
        //Chase the player, if seen from any state
        if (checkingForPlayer == true && WithinRange(sightRange))
        {
            currentState = States.CHASING;
        }

        //Change to MELEE state from any state if the player gets too close
        //(NOTE TO SELF)Commented out as it is not being coded yet
        /*if (WithinRange(meleeRange))
        {
            currentState = States.MELEE;
        }*/

        //Rotate to face the player while in sightRange
        //(NOTE TO SELF) check to see if you can lerp the LookAt, so it doesn't snap too the player once in range
        if (WithinRange(sightRange))
        {
            transform.LookAt(player);
        }
    }
    #endregion

    //Contents of the states, and how to change between them.
    #region Behaviour Coroutines


    #region IDLE
    IEnumerator IDLE()
    {
        float timer;
        timer = 0;

        checkingForPlayer = true;

        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            timer++;

            //Time spent remaining IDLE
            if (timer >= 12)
            {
                currentState = States.PATROLLING;
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region PATROLLING
    IEnumerator PATROLLING()
    {
        checkingForPlayer = true;

        agent.SetDestination(nodes[currentNode].position);
        currentNode = Random.Range(0, nodes.Length);

        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = States.IDLE;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region CHASING
    IEnumerator CHASING()
    {
        checkingForPlayer = false;

        while (currentState == States.CHASING)
        {
            if (!WithinRange(sightRange)) currentState = States.IDLE;

            if (WithinRange(summonRange)) currentState = States.COMBAT;

            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region COMBAT
    IEnumerator COMBAT()
    {
        checkingForPlayer = false;

        //Spawn Elemental at the summonLocation and rotation
        //(NOTE TO SELF)Spawning WAY too many at the moment lol
        //Instantiate(objectToSummon, summonLocation.position, summonLocation.rotation);


        //put any code here you want to repeat during the state being active
        while (currentState == States.COMBAT)
        {
            if (!WithinRange(summonRange))
            {
                currentState = States.CHASING;
            }

            //(NOTE TO SELF) work in progress, partly what I want, but not quite
            agent.SetDestination(player.position * bufferRange);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion

    #region MELEE
    IEnumerator MELEE()
    {
        checkingForPlayer = false;

        //ENTER THE MELEE STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE MELEE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.MELEE)
        {

            yield return null;
        }

        //EXIT MELEE STATE >
        //write any code here you want to run when the state is left
    }
    #endregion


    #endregion

    //Used to help visualize the range values in scene.
    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, meleeRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, dangerRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, summonRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, bufferRange);
    }
    #endregion

    //Determine whether the player is within a certain range
    bool WithinRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
