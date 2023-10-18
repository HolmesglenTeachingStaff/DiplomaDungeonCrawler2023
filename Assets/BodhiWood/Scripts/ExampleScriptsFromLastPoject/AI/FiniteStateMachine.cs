using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    #region Gizmos/Colour Variables
    [Header("Gizmos")]
    public float sightRange;
    public float meleeRange;

    [Header("Colours")]
    public Color sightColor;
    #endregion

    #region State Variables
    //Declare states. If you add a new state to you character, remember to add a new state enum for it.
    public enum States {IDLE, CHASING, ATTACKING}
    public States currentState;

    public Transform player;
    private NavMeshAgent agent;
    #endregion

    #region Initialization
    //Set default state
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //Start the FSM
        StartCoroutine(EnemyFSM());
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
        //ENTER THE IDLE STATE 
        //Put any code here that you want to run at the start of the behaviour.
        Debug.Log("IDLE");

        //UPDATE IDLE STATE
        //Put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            //Check player distance
            if (Vector3.Distance(transform.position, player.position) < sightRange)
            {
                //Change state
                currentState = States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE
        //Write any code here you want to run when the state is left
        Debug.Log("LEAVING IDLE");
    }
    IEnumerator CHASING()
    {
        //ENTER THE CHASING STATE 
        //Put any code here that you want to run at the start of the behaviour.
        Debug.Log("CHASING");

        //UPDATE CHASING STATE
        //Put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("LOST TARGET");
            }
            if (Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
                Debug.Log("FOUND THE TARGET");
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT CHASING STATE
        //Write any code here you want to run when the state is left
        Debug.Log("LEAVING CHASING");
    }
    IEnumerator ATTACKING()
    {
        Debug.Log("ATTACKING");

        while (currentState == States.ATTACKING)
        {
            Debug.Log("HIT!");
            yield return new WaitForSeconds(0.5f);

            if (Vector3.Distance(transform.position, player.position) > meleeRange)
            {
                currentState = States.IDLE;
                Debug.Log("TARGET OUT OF RANGE");
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("LEAVING ATTCKING");
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
