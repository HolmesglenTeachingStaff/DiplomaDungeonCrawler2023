using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]

public class OttHeal_StateMachine_Test : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float healRange;
    public Transform playerToHeal;
    private NavMeshAgent otter_Agent;
    //public Transform[] wayPoints;
    //int currentWaypoint = 0;
    //public float ottSpeed = 10.0f;

   
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, ROAMING, HEALING, RETURNTOSTALL }

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
        otter_Agent = GetComponent<NavMeshAgent>();
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
        while (currentState == States.IDLE)
        {

            //UPDATE IDLE STATE >
            //put any code here you want to repeat during the state being active


            //EXIT IDLE STATE >
            //write any code here you want to run when the state is left
            if (Vector3.Distance(transform.position, playerToHeal.position) < sightRange)
            {
                currentState = States.RETURNTOSTALL;
            }

            else if (Vector3.Distance(transform.position, playerToHeal.position) > sightRange)
            {
                currentState = States.ROAMING;
            }

            yield return new WaitForEndOfFrame();
        }

    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.HEALING)
        {

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator ROAMING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        //while (currentState == States.ROAMING)
        //{

       

       // }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left


        yield return new WaitForSeconds(3);
        Debug.Log("Oh no I see the player!");
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, healRange);

    }

}