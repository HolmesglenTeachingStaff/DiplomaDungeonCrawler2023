using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CS_ShinigamiStateMachine : MonoBehaviour
{
    #region Variables
    public Color sightColor;
    public Color talkColor;
    public float sightRange;
    public float talkRange;
    [SerializeField] float elapsedTime;
    public float setTime;

    public Transform player;
    private NavMeshAgent agent;

    //patrol settings 
    [SerializeField] Transform[] nodes;
    int currentNode;

    Stats stat;

    //Variable for player 
    public GameObject playerChar;
    float playerCurrentHealth;

    //Variables for dialogue 
    CS_DialogueText dialogueText;
    [SerializeField] float messageCount;
    public bool HasDisplayed;


    #endregion

    #region States
    //Declaring states 
    public enum States { IDLE, ROAMING, TALKING, HEAL }
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
        stat = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        dialogueText = GetComponent<CS_DialogueText>();
        //Start the statemachine
        StartCoroutine(TsukuyomiFSM());
    }
    #endregion

    #region FiniteStateMachine
    IEnumerator TsukuyomiFSM()
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
        while (currentState == States.IDLE)
        {
            //Enter Idle state
            Debug.Log("Entered Tsukuyomi Idle state");
            //Wait for a few seconds 
            yield return new WaitForSeconds(Random.Range(1, 5));

            //Play idle annimations
            //anim.Play("");

            //Wait for a few seconds
            yield return new WaitForSeconds(Random.Range(1, 5));

            //transitions to roaming state 
            currentState = States.ROAMING;

            if (IsInRange(talkRange))
            {
                currentState = States.TALKING;
                agent.SetDestination(transform.position);
            }
        }
    }

    IEnumerator ROAMING()
    {
        //Enter roaming state 
        Debug.Log("Tsukuyomi Roaming state activated");

        agent.SetDestination(nodes[currentNode].position);

        //Checks if player is nearby
        while (currentState == States.ROAMING)
        {
            //Roaming behaviour 
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                //yield return new WaitForSeconds(Random.Range(3, 5));
                agent.speed = Random.Range(5, 10);

                currentNode = Random.Range(0, nodes.Length);

                agent.SetDestination(nodes[currentNode].position);
            }

            if (IsInRange(sightRange) || IsInRange(talkRange))
            {
                //Stop current animations
                agent.SetDestination(transform.position);
            }

            if (IsInRange(talkRange))
            {
                currentState = States.TALKING;
            }

            if(elapsedTime >= setTime)
            {
                Debug.Log("Time out");
                agent.SetDestination(transform.position);
                elapsedTime = 0;
                currentState = States.IDLE;
            }
            yield return new WaitForEndOfFrame();
        }
    }
  

    IEnumerator TALKING()
    {
        //Dialogue system with player 
        //Displays UI instruction for player to follow 
        //Plays talking function from different script 
        agent.SetDestination(transform.position);
        while (currentState == States.TALKING)
        {
            if (IsInRange(talkRange))
            {
                dialogueText.canTalk = true;
            }

            if(!IsInRange(talkRange) || !IsInRange(sightRange))
            {
                currentState = States.ROAMING;
                dialogueText.canTalk = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Extra functions 
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

    void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = talkColor;
        Gizmos.DrawWireSphere(transform.position, talkRange);
    }
    #endregion
}
