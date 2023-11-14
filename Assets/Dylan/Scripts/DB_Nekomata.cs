using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DB_Nekomata : MonoBehaviour
{
    #region varaibales
    public NavMeshAgent agent;
    public Transform player;
    private Animator anim;
    public float lastAttack, timeBetweenAttacks, chargeTime, attackSpeed;

    public int hitCounter, maxHits;
    public float hitRecovery;
    private float lastHit;
    private Stats stats;
    
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public CanvasGroup healthSliders;
    #endregion

    #region states
    public enum States { IDLE, CHASING, ATTACKING, DASH, DIE}
    public States currentState;
    #endregion

    #region Initialization
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PLayer").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<Stats>();
        healthSliders.alpha = 0;
        //start fsm
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
        //enter idle state
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CHASING()
    {
        yield return new WaitForEndOfFrame();
        agent.updatePosition = true;
        agent.updatePosition = false;

        agent.speed = 3.5f;
        agent.speed *= 0.25f;

        while (currentState == States.CHASING)
        {
            if (healthSliders.alpha < 1)
            {
                healthSliders.alpha += Time.deltaTime;
            }
        }
    }


    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
