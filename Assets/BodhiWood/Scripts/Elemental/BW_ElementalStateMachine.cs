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
    //GetComponent Variables.
    private NavMeshAgent agent;
    private Animator anim;

    //misc.
    public Transform player;
    public bool checkingForPlayer = false;
    public Collider weaponCollider;

    [Header("Reaction Range Values")]
    public float sightRange = 6;
    public float meleeRange = 2;
    #endregion

    //The behaviour states available for a Summoner to switch between.
    #region States
    public enum States {IDLE, COMBAT, MELEE, DEATH}

    public States currentState;
    #endregion

    //Starting all required components.
    #region Initialization
    void Awake()
    {
        currentState = States.COMBAT;
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        weaponCollider.enabled = false;

        StartCoroutine(ElementalFSM());
    }
    #endregion

    //Initialize the state machine.
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
        //Chase the player, if seen from any state.
        if (checkingForPlayer == true && WithinRange(sightRange))
        {
            currentState = States.COMBAT;
        }

        //Change to MELEE state from any state if the player gets too close.
        if (WithinRange(meleeRange))
        {
            currentState = States.MELEE;
        }

        //Rotate to face the player while in sightRange.
        if (WithinRange(sightRange))
        {
            transform.LookAt(player);
        }
    }
    #endregion

    //Contents of the states, and how to change between them.
    #region Behaviour Coroutines


    #region IDLE
    IEnumerator IDLE()
    {
        float timer;
        timer = 0;

        checkingForPlayer = true;

        while (currentState == States.IDLE)
        {
            //Increases by 1 every second
            timer += Time.deltaTime;

            //Time remaining until this object de-spawns
            if (timer >= 15)
            {
                anim.Play("Elemental_Dead");
                weaponCollider.enabled = false;
                yield return new WaitForSeconds(0.4f);
                Destroy(gameObject);
                StopAllCoroutines();
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
            //Enter IDLE if player is lost.
            if (!WithinRange(sightRange)) currentState = States.IDLE;

            //Chase the player while in COMBAT.
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

        //Stop movement.
        agent.SetDestination(agent.transform.position);

        //Attack the player, with added delays to suit the attack animation.
        anim.Play("Elemental_Melee");
        yield return new WaitForSeconds(0.5f);
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        weaponCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);

        currentState = States.COMBAT;

        while (currentState == States.MELEE)
        {
            //Return to COMBAT if player is lost.
            if (!WithinRange(meleeRange)) currentState = States.COMBAT;

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion

    #region DEATH
    IEnumerator DEATH()
    {
        //Destroys this NPC after a short delay and animation are run.
        anim.Play("Elemental_Dead");
        weaponCollider.enabled = false;
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        StopAllCoroutines();

        yield return null;
    }
    #endregion


    #endregion

    //Function called from the inspector using the OnDeath Event in the Stats script.
    public void Dead()
    {
        StartCoroutine(DEATH());
    }

    //Used to help visualize the range values in scene.
    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, meleeRange);
    }
    #endregion

    //Determine whether the player is within a certain range.
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
