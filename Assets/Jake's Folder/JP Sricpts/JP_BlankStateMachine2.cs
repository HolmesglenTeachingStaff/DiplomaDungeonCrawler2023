using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class JP_BlankStateMachine2 : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    public UnityEvent OnAttack;
    public UnityEvent OffAttack;

    [HideInInspector]
    public Color sightColor;

    [SerializeField] Transform[] nodes;
    int currentNode;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING }

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
        agent = GetComponent<NavMeshAgent>();
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
        yield return new WaitForSeconds(2);
        currentState = States.PATROLLING;
       

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            
            yield return new WaitForSeconds(1);
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        
    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position);
            agent.speed=4;
            OnAttack.Invoke();
            yield return new WaitForSeconds(0.75f);
            agent.speed=0;
            OffAttack.Invoke();
            yield return new WaitForSeconds(0.5f);
            if(sightRange<Vector3.Distance(player.position,transform.position))
             {
                currentState=States.PATROLLING;
             }
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator PATROLLING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            if(!agent.pathPending && agent.remainingDistance >= agent.stoppingDistance)
             {
                agent.speed=4;
                agent.SetDestination(nodes[currentNode].position);
                yield return new WaitForSeconds(0.75f);
                agent.speed=0;
                yield return new WaitForSeconds(0.5f);
             }
             if(!agent.pathPending && agent.remainingDistance == agent.stoppingDistance)
             {
                currentNode = (currentNode + 1) % nodes.Length;
                agent.speed=3;
             }
             if(sightRange>Vector3.Distance(player.position,transform.position))
             {
                currentState=States.CHASING;
             }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

}