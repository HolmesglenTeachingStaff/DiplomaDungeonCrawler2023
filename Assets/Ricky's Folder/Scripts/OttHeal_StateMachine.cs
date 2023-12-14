using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class OttHeal_StateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float healRange;
    public Transform playerToHeal;
    private NavMeshAgent ott_Agent;
    public Transform ott_HealHome;
    public float waitBeforeChangingState = 2.5f;


    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States
    {
        IDLE,
        ROAMING,
        WAITINGTOHEAL,
        HEALING 
    }

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
        ott_Agent = GetComponent<NavMeshAgent>();
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

       

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            if (Vector3.Distance(transform.position, playerToHeal.position) < sightRange)
            {
                currentState = States.WAITINGTOHEAL;
            }







            yield return new WaitForSeconds(1);
            
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        
    }
    IEnumerator WAITINGTOHEAL()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        ott_Agent.transform.position = ott_HealHome.position;


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.WAITINGTOHEAL)
        {
           


            if(Vector3.Distance(transform.position, playerToHeal.position) < healRange)
            {
                currentState=States.HEALING;
            }

            if(Vector3.Distance(transform.position, playerToHeal.position) > sightRange)
            {
                currentState = States.IDLE;
            }
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
        while (currentState == States.ROAMING)
        {
            if(Vector3.Distance(transform.position, playerToHeal.position) < sightRange)
            {
                currentState = States.WAITINGTOHEAL;
            }
            
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
        yield return new WaitForEndOfFrame();
    }
    IEnumerator HEALING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.HEALING)
        {
            if(Vector3.Distance(transform.position, playerToHeal.position) > healRange)
            {
                currentState = States.IDLE;
            }
            
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

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