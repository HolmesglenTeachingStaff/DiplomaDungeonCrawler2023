using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NT_OniStateMachine : MonoBehaviour
{
    #region Variables
    public float sightRange;
    public float meleeRange;

    public Transform player;
    private NavMeshAgent agent;

    private Animator anim;

    //patrol settings 
    [SerializeField] Transform[] nodes;
    int currentNode;

    //Variables for health when in retreating and restore state 
    //need ref to stats script to effect health 

    #endregion

    #region States
    //Declaring states 
    public enum States { IDLE, ROAMING, CHASING, ATTACKING, RETREAT}
    public States currentState;
    #endregion

    #region Initialization
    //Setting a default state

    private void Awake()
    {
        currentState = States.IDLE;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //Start the statmachine
        StartCoroutine(OniFSM()); 
    }
    #endregion

    #region FiniteStateMachine
    IEnumerator OniFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Behaviour coroutines
    IEnumerator IDLE()
    {
        //Enter Idle state
        Debug.Log("Entered Idle state");
        //Wait for a few seconds 
        yield return new WaitForSeconds(Random.Range (1,5));

        //Play idle annimations
        anim.Play("");

        //Wait for a few seconds
        yield return new WaitForSeconds(Random.Range(1, 5));

        //transitions to roaming state 
        agent.SetDestination(transform.position);
        currentState = States.ROAMING;

        //Check if player is in range 
        while(currentState == States.IDLE)
        {
            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Player found will start chasing mode");
    }

    IEnumerator CHASING()
    {
        //Enter chasing state 
        Debug.Log("Chasing mode activated");

        //Chasing state behaviour 
        while (currentState == States.CHASING)
        {
            agent.SetDestination(transform.position);

            if (!IsInRange(sightRange))
            {
                currentState = States.IDLE;
            }
            else
            {
                if (IsInRange(meleeRange))
                {
                    currentState = States.ATTACKING;
                }
            }
            agent.SetDestination(transform.position);
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator ROAMING()
    {
        //Enter roaming state 
        Debug.Log("Roaming state activated");

        agent.SetDestination(nodes[currentNode].position);

        //Checks if player is nearby
        while(currentState == States.ROAMING)
        {
            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }
            //Roaming behaviour 
            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                yield return new WaitForSeconds(Random.Range(3, 5));
                agent.speed = Random.Range(5, 10);

                currentNode = Random.Range(0, nodes.Length);

                agent.SetDestination(nodes[currentNode].position);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ATTACKING()
    {
        //Play attacking animations 
        //Check if player is still in melee range 
        //switch states if not 
        //If player is still in melee range, keep attacking until health reaches below 85% 
        //Switch to retreat state
    }

    IEnumerator RETREAT()
    {
        //Character goes to furthest node from player
        //Once reached node, immeadiately heal health back to 100% 
        //After health is back to 100% checks for player in sight 
        //Switches back to roaming if player is not in sight 
        //Switches to chasing/attacking if player is in sight 
    }

    #endregion

    #region Extra functions 
    bool IsInRange (float range)
    {
        if(Vector3.Distance(player.position, transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
