using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class OttBuff_StateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float buffRange;
    public Transform playerToBuff;
    private NavMeshAgent ott_BuffAgent;
    public Transform ott_BuffHome;
    public Stats playerStats;

    public float range;
    public Transform centerPoint;


    public Animator ottBuff_Anim;


    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States
    {
        IDLE,
        ROAMING,
        WAITINGTOBUFF,
        BUFFING,
        MOVETOSTALL,
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
        ott_BuffAgent = GetComponent<NavMeshAgent>();
        ottBuff_Anim = GetComponent<Animator>();
        playerStats = GetComponent<Stats>();
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
            if (Vector3.Distance(transform.position, playerToBuff.position) < sightRange)
            {
                
                currentState = States.MOVETOSTALL;
            }

            else if(Vector3.Distance(transform.position, playerToBuff.position) > sightRange)
            {
                yield return new WaitForSeconds(3);
                currentState = States.ROAMING;
            }





            yield return new WaitForEndOfFrame();
            
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        
    }
    IEnumerator WAITINGTOBUFF()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
       


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.WAITINGTOBUFF)
        {
            transform.LookAt(playerToBuff);


            if (Vector3.Distance(transform.position, playerToBuff.position) < buffRange)
            {
                currentState=States.BUFFING;
            }

            if(Vector3.Distance(transform.position, playerToBuff.position) > sightRange)
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
        //ENTER THE Roaming STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Roaming STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.ROAMING)
        {
            if(Vector3.Distance(transform.position, playerToBuff.position) < sightRange)
            {
                currentState = States.MOVETOSTALL;
            }

            else if (Vector3.Distance(transform.position,playerToBuff.position) > sightRange)
            {
                //yield return new WaitForSeconds(5);
                if (ott_BuffAgent.remainingDistance <= ott_BuffAgent.stoppingDistance) //done with path
                {
                    Vector3 point;
                    if (RandomPoint(centerPoint.position, range, out point)) //pass in our centre point and radius of area
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        ott_BuffAgent.SetDestination(point);
                        currentState = States.IDLE;
                    }
                }

                
            }
            
        }

        //EXIT Roaming STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
        yield return new WaitForEndOfFrame();
    }
    IEnumerator HEALING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerStats.currentHealth += 100f;
        }

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.BUFFING)
        {
            if(Vector3.Distance(transform.position, playerToBuff.position) > buffRange)
            {
                currentState = States.WAITINGTOBUFF;
            }








            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    } 
    IEnumerator MOVETOSTALL()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        transform.LookAt(ott_BuffHome);
        ott_BuffAgent.SetDestination(ott_BuffHome.position);


        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.MOVETOSTALL)
        {
            if(Vector3.Distance(transform.position, playerToBuff.position) < sightRange)
            {
                currentState = States.WAITINGTOBUFF;
            }
            
            if(Vector3.Distance(transform.position, playerToBuff.position) > sightRange)
            {
                currentState= States.IDLE;
            }

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    #endregion


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buffRange);
    }

}