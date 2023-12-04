using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieryWisp_States : MonoBehaviour
{
    /// <summary>
    /// FiniteStateMachine for FieryWisp
    /// </summary>

    #region variables
    Stats stats;

    [Header ("Variables")]
    public float sightRange;
    public float attackRange;
    public float attackDelay;

    [Header ("Objects")]
    public GameObject player;
    public Animation anim;
    public GameObject hitBox;

    [Header("Territory")]
    public Transform spawnPoint;
    public float chaseRange; //the area this object can follow player

    private NavMeshAgent agent;

    [Header ("Gizmos")]
    public Color sightColor;
    public Color chaseColor;
    public Color attackColor;
    #endregion

    #region States
    public enum States {IDLE, CHASING, ATTACKING, RETURN }
    public States currentState;

    private void Awake()
    {
        currentState = States.IDLE;
    }
    void Start()
    {
        stats = GetComponent<Stats> ();
        agent.speed = stats.currentSpeed;

        hitBox.SetActive (false); //hitbox failsafe
        this.transform.position = spawnPoint.position; //spawn on spawnpoint
        StartCoroutine(EnemyFSM());
    }
    #endregion

    #region FiniteStateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Coroutines
    IEnumerator IDLE() //transitions to Chasing
    {
        anim.Play("Idle"); //play idle anim
        while (currentState == States.IDLE)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightRange) //player in sight
            {
                currentState = States.CHASING;
                yield return StartCoroutine(currentState.ToString());
            }
        }
        yield return new WaitForEndOfFrame();
    }
    
    IEnumerator CHASING() //transitions to return or attacking
    {
        anim.Play("Moving");
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange) //can attack
            {
                currentState = States.ATTACKING;
                yield return StartCoroutine(currentState.ToString());
            }
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator ATTACKING()
    {
        //play attacking animation
        anim.Play("Attacking");
        new WaitForSeconds(attackDelay); //attack delay before explosion
        hitBox.SetActive(true);
        new WaitForSeconds(.3f);
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();
    }

    IEnumerator RETURN()
    {
        anim.Play("Moving");
        yield return StartCoroutine(currentState.ToString());
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = chaseColor;
        Gizmos.DrawWireSphere(spawnPoint.position, chaseRange);
    }
    #endregion
}