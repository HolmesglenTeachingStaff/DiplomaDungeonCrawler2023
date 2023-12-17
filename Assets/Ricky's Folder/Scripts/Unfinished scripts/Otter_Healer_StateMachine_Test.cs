using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Otter_Healer_StateMachine_Test : MonoBehaviour
{
    #region Variables
    public float sightRange;
    public float healingRange;
    public Transform playerToHeal;
    private NavMeshAgent navMeshOtterHealer;
    public Transform healerHome;
    public Transform target;
    public Transform []wayPoints;
    public float roamRadius = 10f;

    public Vector3 targetPoint;
    int wayPointIndex;








    #endregion


    #region States
    //Declare States. If need to add more states, Remember to add a new states enum for it
    public enum States { IDLE, ROAMING, BACKTOHOME, HEALING, WAITINGTOHEAL, LOOKINGAROUND };
    public States currentState;


    #endregion


    #region Initialization
    // Set Default State
    private void Awake()
    {
        currentState = States.IDLE;
    }

    #endregion

    private void Start()
    {
        navMeshOtterHealer = GetComponent<NavMeshAgent>();

        StartCoroutine(FriendlyFSM());
    }


    #region Finite State Machine
    IEnumerator FriendlyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
        
    }

    #endregion

    #region Behaviour Coriutines
    IEnumerator IDLE()
    {
        // Enter Idle State
        Debug.Log("I'm not seeing anyone at the moment");

        // Update Idle State
        while(currentState == States.IDLE)
        {
            // Check player Distance to OtterHealer
            if(Vector3.Distance(transform.position, playerToHeal.position) < sightRange)
            {
                // Change state
                currentState = States.BACKTOHOME;
            }

            if (Vector3.Distance(transform.position, playerToHeal.position) > sightRange)
            {
                currentState = States.ROAMING;
            }
            yield return new WaitForSeconds(3f);

        }

        // Exit Idle State
        Debug.Log("OH! There is someone, I better get back to my stall!");

    } 
    
    IEnumerator ROAMING()
    {
        // Implement Roaming logic
        if (!navMeshOtterHealer.pathPending && navMeshOtterHealer.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas);
            navMeshOtterHealer.SetDestination(hit.position);
        }
        return null;


        // Enter Roaming State
        //Debug.Log("Might as well walk around, See if I can find anyone");

        // Update Roaming State
        //while(currentState == States.ROAMING)
        //{
        //targetPoint = wayPoints[wayPointIndex].position;
        //navMeshOtterHealer.SetDestination(targetPoint);



        //if(Vector3.Distance(transform.position, playerToHeal.position) < sightRange)
        //{
        //    currentState = States.BACKTOHOME;
        //}
        //else if (Vector3.Distance(transform.position, playerToHeal.position) > sightRange)
        //{
        //    currentState = States.IDLE;
        //}
        // yield return null;
        //}

        // Exit ROAMING State
        //Debug.Log("OH! There is someone, I better get back to my stall!");


    } 
    
    //IEnumerator BACKTOHOME()
    //{
    //    while (currentState == States.BACKTOHOME)
        //{




        //    return null;
        //}





    //}
    IEnumerator HEALING()
    {
        // Enter Healing State
        Debug.Log("Don't worry buddy, I will have you fixed up in no time");

        // Update Healing State
        while(currentState == States.HEALING)
        {
            
            if(Vector3.Distance(transform.position, playerToHeal.position) < healingRange)
            {
                
            }

            yield return null;
        }

        // Exit BackToHome State
        Debug.Log("Alright, Time to look pretty and help if they need it!");


    }
    IEnumerator WAITINGTOHEAL()
    {
        // Enter WaitingToHeal State
        Debug.Log("Don't worry buddy, I will have you fixed up in no time");

        // Update WaitingToHeal State
        while(currentState == States.WAITINGTOHEAL)
        {
            
            if(Vector3.Distance(transform.position, playerToHeal.position) < healingRange)
            {
                currentState = States.HEALING;              
            }

            else if (Vector3.Distance(transform.position,playerToHeal.position) > sightRange)
            {
                currentState = States.IDLE;
            }

            yield return null;
        }

        // Exit BackToHome State
        Debug.Log("Alright, Time to look pretty and help if they need it!");


    }
    IEnumerator LOOKINGAROUND()
    {
        // Enter WaitingToHeal State
        Debug.Log("Don't worry buddy, I will have you fixed up in no time");

        // Update WaitingToHeal State
        while(currentState == States.LOOKINGAROUND)
        {
            // Implement LookingAround logic
            currentState = States.IDLE; // Stop moving
            yield return new WaitForSeconds(3f); // Look around for 3 seconds
            currentState = States.ROAMING; // Go back to Roaming state


        }

        // Exit BackToHome State
        Debug.Log("Alright, Time to look pretty and help if they need it!");


    }


    #endregion


 

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);


        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, healingRange);

    }




 

}
