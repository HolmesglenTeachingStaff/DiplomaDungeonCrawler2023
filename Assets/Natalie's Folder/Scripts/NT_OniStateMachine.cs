using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NT_OniStateMachine : MonoBehaviour
{
    #region Variables
    public Color sightColor;
    public Color meleeColor;
    //public Color nodeRangeColor;
    public float sightRange;
    public float meleeRange;
    [SerializeField] float elapsedTime;

    public Transform player;
    public GameObject oniWeapon;
    private NavMeshAgent agent;

    //private Animator anim;

    //patrol settings 
    [SerializeField] Transform[] nodes;
    int currentNode;

    //Variables for health when in retreating and restore state 
    //need ref to stats script to effect health 
    Stats stat;
    DamageOnTouch damageOnTouch;

    #endregion

    #region States
    //Declaring states 
    public enum States { IDLE, ROAMING, CHASING, ATTACKING, RETREAT , DEATH}
    public States currentState;
    #endregion

    #region Initialization
    //Setting a default state

    private void Awake()
    {
        currentState = States.IDLE;
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    private void Start()
    {
        stat = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        //Start the statemachine
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
        //Check if player is in range 
        while (currentState == States.IDLE)
        {
            //Enter Idle state
            Debug.Log("Entered Idle state");
            //Wait for a few seconds 
            yield return new WaitForSeconds(Random.Range(1, 5));

            //Play idle annimations
            //anim.Play("");

            //Wait for a few seconds
            yield return new WaitForSeconds(Random.Range(1, 5));

            //transitions to roaming state 
            currentState = States.ROAMING;

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
            agent.SetDestination(player.position);

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
            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator ROAMING()
    {
        //Enter roaming state 
        Debug.Log("Roaming state activated");

        agent.SetDestination(nodes[currentNode].position);

        //Checks if player is nearby
        while (currentState == States.ROAMING)
        {
            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }
            //Roaming behaviour 
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                //yield return new WaitForSeconds(Random.Range(3, 5));
                agent.speed = Random.Range(5, 10);

                currentNode = Random.Range(0, nodes.Length);

                agent.SetDestination(nodes[currentNode].position);
                yield return new WaitForSeconds(3);
            }

            if(elapsedTime >= 29)
            {
                Debug.Log("Time out");
                agent.SetDestination(transform.position);
                elapsedTime = 0;
                currentState = States.IDLE;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ATTACKING()
    { 
        //Play attacking animations 
        while (currentState == States.ATTACKING)
        {
           //anim.Play("");
           //Toggles off collider on weapon 
           //activates the damage on touch 
           if(!IsInRange(meleeRange))
           {
               currentState = States.CHASING;
           }

           if(stat.currentHealth <= 15)
           {
               currentState = States.RETREAT;
           }

            if (stat.currentHealth <= 0)
            {
                currentState = States.DEATH;
            }
            yield return new WaitForEndOfFrame();
        }
        //Check if player is still in melee range 
        //switch states if not 
        //If player is still in melee range, keep attacking until health reaches below 85% 
        //Switch to retreat state
    }

    IEnumerator RETREAT()
    {
        //Current bug: NPC doesnt go to node at 15% health, instead heals on the spot while fighting player 
        //Character goes to furthest node from player

        while (currentState == States.RETREAT)
        {
            bool hasHealed = false;
            bool hasWaypoint = false;

            if (IsInRange(meleeRange) && IsInRange(sightRange) && stat.currentHealth <= 25)
            {
                //Once reached node, immeadiately heal health back to 100%
                if(hasWaypoint == false)
                {
                    agent.speed = 15;
                    currentNode = Random.Range(0, nodes.Length);
                    agent.SetDestination(nodes[currentNode].position);
                    hasWaypoint = true;
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        //Once reached node, immeadiately heal health back to 100%
                        //Once reached node, haswaypoint is true
                        stat.currentHealth += 75;
                        stat.OnBuffRecieved.Invoke();
                        hasHealed = true;
                    }
                }
                else
                {
                    hasWaypoint = false;
                }

            }

            if(stat.currentHealth <= 0)
            {
                currentState = States.DEATH;
            }

            //After health is back to 100% checks for player in sight
            if (IsInRange(sightRange) && hasHealed == true)
            {
                currentState = States.CHASING;
            }
            else if(!IsInRange(sightRange) && hasHealed == true)
            {
                currentState = States.ROAMING;
            }
            //Switches back to roaming if player is not in sight 
            //Switches to chasing/attacking if player is in sight 
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DEATH()
    {
        //Once in this state, NPC will destroy itself and play shader
        //Invoke die function from stat script 
        //Use shader for death 

        while (currentState == States.DEATH)
        {
            if(stat.currentHealth <= 0)
            {
                Destroy(gameObject);
                stat.Die();
            }
            yield return new WaitForEndOfFrame();
        }
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

    void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = meleeColor;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        //Gizmos.color = nodeRangeColor;
        //Gizmos.DrawWireSphere(transform.position, nodeRange);
    }
    #endregion
}
