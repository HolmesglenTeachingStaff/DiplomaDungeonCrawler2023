using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class BlankStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    private Animator anim;

    [HideInInspector]
    public Color sightColor;

    //Patrol settings
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
        anim = GetComponent<Animator>();
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



    IEnumerator CHASING()
    {
        //ENTER THE CHASING STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE CHASING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            /*if (Vector3.Distance(player.position, transform.position) > sightRange)
            {
                currentState = States.IDLE;
            }
            else if (Vector3.Distance(player.position, transform.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }*/
            if (!IsInRange(sightRange))
            {
                currentState = States.IDLE;
            }
            else if (IsInRange(meleeRange))
            {
                currentState = States.ATTACKING;
            }

            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }

        //EXIT CHASING STATE >
        //write any code here you want to run when the state is left
    }



    IEnumerator PATROLLING()
    {
        //ENTER THE PATROLLING STATE >
        //put any code here that you want to run at the start of the behaviour

        agent.SetDestination(nodes[currentNode].position);

        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            //very basic chase state change
            /*if(Vector3.Distance(player.position, transform.position) < sightRange)
            {
                currentState = States.CHASING;
            }*/

            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                yield return new WaitForSeconds(Random.Range(3, 10));
                //patrolling vvv
                //currentNode = (currentNode + 1) % nodes.Length;

                //roaming vvv
                currentNode = Random.Range(0, nodes.Length);

                agent.SetDestination(nodes[currentNode].position);
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT PATROLLING STATE >
        //write any code here you want to run when the state is left
    }



    IEnumerator ATTACKING()
    {
        //ENTER THE ATTACKING STATE >
        //put any code here that you want to run at the start of the behaviour

        //play animation
        anim.Play("Darius_Sitting_Laugh");
        //wait
        yield return new WaitForSeconds(8.4f);
        //change state
        currentState = States.CHASING;

        yield return null;
    }
    #endregion

    bool IsInRange(float range)
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

    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
    #endregion
}