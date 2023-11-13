using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RD_NinjaFiniteStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public float rangedRange;
    public Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    public Color sightColor;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, ROAMING, CHASING, MELEEATTACK, RANGEDATTACK, DODGE, FLEEING }
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

        Debug.Log("Alright, seems no evil Player is around, I can chill!");

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            //check player distance
            if (Vector3.Distance(transform.position, player.position) < sightRange)
            {
                //change state
                currentState = States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("aaaargh he's too fast!");
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }

    IEnumerator ROAMING()
    {
        //ENTER THE ROAMING STATE >

        Debug.Log("Where did they go?");

        //UPDATE ROAMING STATAE >
        while (currentState == States.ROAMING)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("aaaargh they're too fast!");
            }
            yield return new WaitForEndOfFrame();
        }


    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
