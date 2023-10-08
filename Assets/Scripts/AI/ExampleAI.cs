using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;
/// <summary>
/// A script that handles the movement of an enemy using states stored as enums and enumerators;
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]

public class ExampleAI : AIWayPoints
{
    [Header("Behaviour Settings")]
    [SerializeField] float sightRange = 10;
    [SerializeField] float attackRange = 1f;
    public enum States { IDLE, CHASE, ATTACK, PATROLLING, DEAD }
    public States currentState;
    public States defaultState;
    NavMeshAgent agent;
    

    [Header("MovementSettings")]
    
    [SerializeField] float patrolSpeed = 3.5f;
    [SerializeField] float chaseSpeed = 5f;
   // public List<Vector3> waypoints = new List<Vector3>();

   // int currentWaypoint = 0;
    public Transform target;

    //other settings
    Ray sightRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        currentState = defaultState;
        agent = GetComponent<NavMeshAgent>();
        
        StartCoroutine(SM());        
    }
    IEnumerator SM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
            /*else
            {
                StopAllCoroutines();
                StartCoroutine(DEAD());
                yield return null;
            }*/
        }
    }
    IEnumerator IDLE()
    {
        //enter state
        
        //loop while state is active
        while (currentState == States.IDLE)
        {
            CheckForPlayer();
            yield return new WaitForSeconds(2);
            currentState = States.PATROLLING;
            yield return null;
        }

        //exit the state
        yield return null;
    }
    IEnumerator CHASE()
    {
        //enter state
        Debug.Log("Chase");
        agent.speed = chaseSpeed;
        agent.SetDestination(target.position);
        //loop while state is active
        while (currentState == States.CHASE)
        {
            CheckForPlayer();
            if(!agent.pathPending && agent.remainingDistance >= agent.stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            yield return new WaitForEndOfFrame();
        }

        //exit the state
        yield return null;
    }
    IEnumerator ATTACK()
    {
        //enter state
        Debug.Log("attack");
        agent.speed = chaseSpeed;
        agent.SetDestination(transform.position);
        //loop while state is active
        while (currentState == States.ATTACK)
        {
            CheckForPlayer();
            //insert your attack behaviour here
            yield return new WaitForEndOfFrame();
        }

        //exit the state
        yield return null;
    }
    IEnumerator PATROLLING()
    {
        //enter state
        Debug.Log("PATROLLING");
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[currentWaypoint]);
        //loop while state is active
        while (currentState == States.PATROLLING)
        {
            CheckForPlayer();
            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                agent.SetDestination(waypoints[currentWaypoint]);
            }
            yield return new WaitForEndOfFrame();
        }

        //exit the state
        yield return null;
    }
    IEnumerator DEAD()
    {
        currentState = States.DEAD;
        //loop while state is active
        while (currentState == States.DEAD)
        {
            //add any death animations here
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
            yield return null;
        }

        //exit the state
        yield return null;
    }
    private void CheckForPlayer()
    {
        var gap = Vector3.Distance(transform.position, target.position);
        sightRay.direction = target.position - transform.position;
        sightRay.origin = transform.position;
        if(gap <= attackRange)
        {
            currentState = States.ATTACK;
        }
        else if(gap <= sightRange)
        {
            currentState = States.CHASE;
        }
        else
        {
            currentState = States.PATROLLING;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        
    }
}
