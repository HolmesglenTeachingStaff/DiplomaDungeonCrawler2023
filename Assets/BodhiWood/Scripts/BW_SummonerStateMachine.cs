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
    public enum States {IDLE, PATROLLING, DEAD, MELEE, CHASING, COMBAT}

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
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
    }
    IEnumerator PATROLLING()
    {
        //ENTER THE PATROLLING STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT PATROLLING STATE >
        //write any code here you want to run when the state is left
    }
    IEnumerator CHASING()
    {
        //ENTER THE CHASING STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE CHASING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT CHASING STATE >
        //write any code here you want to run when the state is left
    }
    IEnumerator COMBAT()
    {
        //ENTER THE COMBAT STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE COMBAT STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT COMBAT STATE >
        //write any code here you want to run when the state is left
    }
    IEnumerator MELEE()
    {
        //ENTER THE MELEE STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE MELEE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT MELEE STATE >
        //write any code here you want to run when the state is left
    }
    IEnumerator DEAD()
    {
        //ENTER THE DEAD STATE >
        //put any code here that you want to run at the start of the behaviour

        yield return new WaitForSeconds(6);

        currentState = States.PATROLLING;
        //UPDATE DEAD STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {

            yield return null;
        }

        //EXIT DEAD STATE >
        //write any code here you want to run when the state is left
    }
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
}
