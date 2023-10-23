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
    public Transform lookAt;

    public GameObject objectToSummon;

    [Header("Reaction Range Values")]
    public float sightRange;
    public float summonRange;
    public float dangerRange;
    public float meleeRange;

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

    //Contents of the states, and how to change between them.
    #region Behaviour Coroutines


    #region IDLE
    IEnumerator IDLE()
    {
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            if (Vector3.Distance(player.position, transform.position) < sightRange)
            {
                currentState = States.CHASING;
            }

            //Currently trying to exit out of WaitForSeconds early if player enters range, before WaitForSeconds has ended

            else if (Vector3.Distance(player.position, transform.position) > sightRange)
            {
                yield return new WaitForSeconds(Random.Range(5, 10));

                currentState = States.PATROLLING;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region PATROLLING
    IEnumerator PATROLLING()
    {
        agent.SetDestination(nodes[currentNode].position);
        currentNode = Random.Range(0, nodes.Length);

        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            if (Vector3.Distance(player.position, transform.position) < sightRange)
            {
                currentState = States.CHASING;
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = States.IDLE;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region CHASING
    IEnumerator CHASING()
    {
        //ENTER THE CHASING STATE >
        //put any code here that you want to run at the start of the behaviour

        //UPDATE CHASING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            if (!WithinRange(sightRange)) currentState = States.IDLE;

            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }

        //EXIT CHASING STATE >
        //write any code here you want to run when the state is left
    }
    #endregion

    #region COMBAT
    IEnumerator COMBAT()
    {
        //ENTER THE COMBAT STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE COMBAT STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.COMBAT)
        {

            yield return null;
        }

        //EXIT COMBAT STATE >
        //write any code here you want to run when the state is left
    }
    #endregion

    #region MELEE
    IEnumerator MELEE()
    {
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
