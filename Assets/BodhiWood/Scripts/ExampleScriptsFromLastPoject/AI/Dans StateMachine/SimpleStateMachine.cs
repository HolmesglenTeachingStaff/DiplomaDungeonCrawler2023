using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class SimpleStateMachine : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    public float sightRadius;

    [SerializeField] Transform[] nodes;
    int currentNode = 0;

    public enum STATES {IDLE, ROAMING, CHASING, ATTACKING}
    public STATES currentState;

    private void Awake()
    {
        currentState = STATES.IDLE;
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(EnemyFSM());
    }

    #region State machine Coroutines
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator IDLE()
    {
        //ENTER THE STATE: run behaviours on state start here
        Debug.Log("Toot toot. I can't move or I'll....");
        int idleTime = 0;

        //EXECUTE STATE: run the main behaviour of the state here
        while(currentState == STATES.IDLE)
        {
            yield return new WaitForSeconds(1);
            idleTime += 1;
            Debug.Log("..... mmmm but my job is to guard.... but I did eat mexican....");

            yield return new WaitForSeconds(1);
            idleTime += 1;
            Debug.Log("... I sure hope no bad guys show up. I don't know if I can handle chasing anyone on this tummy..");

            if(idleTime >= 6)
            {
                currentState = STATES.ROAMING;
            }
        }

        //EXIT THE STATE: run anything when the the state is finished
        Debug.Log("Oohooooh no I see someone");
    }

    IEnumerator CHASING()
    {
        //ENTER THE STATE: run behaviours on state start here
        Debug.Log("*Farting noises* oh no, please don't make me run");

        //EXECUTE STATE: run the main behaviour of the state here
        while (currentState == STATES.CHASING)
        {
            agent.SetDestination(target.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = STATES.IDLE;
            }
            

            yield return new WaitForEndOfFrame();
            
        }

        //EXIT THE STATE: run anything when the the state is finished
        agent.ResetPath();
        Debug.Log("few, I need to sit back down");
    }

    IEnumerator ROAMING()
    {
        //ENTER THE STATE: run behaviours on state start here
        Debug.Log("OOOOOO I guess I'll go to work...");
        agent.SetDestination(nodes[currentNode].position);

        //EXECUTE STATE: run the main behaviour of the state here
        while (currentState == STATES.ROAMING)
        {
            CheckSight(STATES.CHASING);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentNode = (currentNode + 1) % nodes.Length;
                agent.SetDestination(nodes[currentNode].position);
            }


            yield return new WaitForEndOfFrame();

        }

        //EXIT THE STATE: run anything when the the state is finished
        agent.ResetPath();
    }

    void CheckSight(STATES stateToEnter)
    {
        if(Vector3.Distance(transform.position, target.position) < sightRadius)
        {
            currentState = stateToEnter;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
    //GO to a specific point
    //Detect something in a radias, do a line of sight check
    //Patrol an area
    //collaps and stagger


    #endregion
}
