using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// State machine for an Elemental, used for managing Elementals states/behaviour.
/// Will hold the majority of code for controlling how an Elemental acts/reacts.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BW_ElementalStateMachine : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    private Animator anim;

    public Transform player;
    public bool checkingForPlayer = false;

    [Header("Reaction Range Values")]
    public float sightRange = 6;
    public float meleeRange = 2;
    #endregion

    #region States
    public enum States {IDLE, COMBAT, MELEE}

    public States currentState;
    #endregion

    #region Initialization
    void Awake()
    {
        currentState = States.COMBAT;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(ElementalFSM());
    }
    #endregion

    #region Finite State Machine
    IEnumerator ElementalFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Update
    void Update()
    {
        //Chase the player, if seen from any state
        if (checkingForPlayer == true && WithinRange(sightRange))
        {
            currentState = States.COMBAT;
        }

        //Change to MELEE state from any state if the player gets too close
        if (WithinRange(meleeRange))
        {
            currentState = States.MELEE;
        }

        //Rotate to face the player while in sightRange
        if (WithinRange(sightRange))
        {
            transform.LookAt(player);
        }
    }
    #endregion

    #region Behaviour Coroutines


    #region IDLE
    IEnumerator IDLE()
    {
        float timer;
        timer = 0;

        checkingForPlayer = true;

        while (currentState == States.IDLE)
        {
            timer += Time.deltaTime;

            //play IDLE animation

            //Time remaining until this object de-spawns
            if (timer >= 15)
            {
                Destroy(gameObject);
            }

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region COMBAT
    IEnumerator COMBAT()
    {
        checkingForPlayer = false;

        while (currentState == States.COMBAT)
        {
            if (!WithinRange(sightRange)) currentState = States.IDLE;

            //play movement animation

            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion

    #region MELEE
    IEnumerator MELEE()
    {
        checkingForPlayer = false;

        agent.SetDestination(agent.transform.position);

        //put any code here you want to repeat during the state being active
        while (currentState == States.MELEE)
        {
            if (!WithinRange(meleeRange)) currentState = States.COMBAT;
            //play animation
            //set active the damage collider

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion


    #endregion

    //Function called from the inspector using the OnDeath Event in the Stats script
    public void Dead()
    {
        //play death animation
        //wait until it's finished
        Destroy(gameObject);
    }

    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, meleeRange);
    }
    #endregion

    #region Range
    bool WithinRange(float range)
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
    #endregion
}
