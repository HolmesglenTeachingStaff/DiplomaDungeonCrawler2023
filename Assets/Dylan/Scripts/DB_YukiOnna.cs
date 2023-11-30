using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DB_YukiOnna : MonoBehaviour
{
    #region varaibales
    private NavMeshAgent agent;
    public Transform player;
    private Animator anim;
    public float lastAttack, timeBetweenAttacks;

    private int hitCounter, maxHits;
    private float hitRecovery;
    private float lastHit;
    private Stats stats;

    public float sightRange, attackRange, slowRange;

    //patrolling
    public List<Vector3> waypoints = new List<Vector3>();
    private int waypointIndex;

    [HideInInspector]
    public Color sightColor;
    #endregion

    #region states
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING, DIE }
    public States currentState;
    #endregion

    #region Initialization
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        GetComponent<Stats>().OnDamaged.Invoke();

        //collect waypoint pos
        foreach (Transform item in transform)
        {
            if (item.tag == "Waypoint")
            {
                waypoints.Add(item.position);
            }
        }
        lastAttack = 0f;
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
        float timer = 0f;

        //update idle state
        while (currentState == States.IDLE)
        {
            //check for player, count until IDLE has run out of time
            if (IsInRange(attackRange)) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else timer += Time.deltaTime;
            if (timer > 5) currentState = States.PATROLLING;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator CHASING()
    {
        //enter CHASING state
        agent.updateRotation = true;
        agent.SetDestination(player.position);

        //update CHASING
        while (currentState == States.CHASING)
        {
            if (IsInRange(attackRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            else agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PATROLLING()
    {
        agent.SetDestination(waypoints[waypointIndex]);
        Debug.Log("Im coming.");

        //update PATROLLING
        while (currentState == States.PATROLLING)
        {
            if (IsInRange(attackRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.ATTACKING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    waypointIndex = waypointIndex + 1 % waypoints.Count;
                }
                agent.SetDestination(waypoints[waypointIndex]);
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("I see you");
    }

    IEnumerator ATTACKING()
    {
        Debug.Log("Ill kill you");
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        lastAttack = Time.time;
        //gamble which attack to run
        int attackType = Random.Range(0, 100);

        if (attackType <= 70)
        {
            anim.SetTrigger("IceSpell");//run animation
            yield return new WaitForSeconds(2);
        }
        else if (attackType > 70)
        {
            anim.SetTrigger("IceSpell");//run animation
            yield return new WaitForSeconds(2);
        }

        agent.updateRotation = true;
        if (IsInRange(sightRange)) currentState = States.CHASING;
        else currentState = States.PATROLLING;

        Debug.Log("I see you");
    }

    IEnumerator DIE()
    {
        //enter DIE state
        agent.speed = 0f;
        agent.SetDestination(transform.position);
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(2f);

        SkinnedMeshRenderer[] models = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer model in models)
        {
            model.enabled = false;
        }
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.enabled = false;
        }
        Destroy(gameObject, 1);

        Debug.Log("youre so mean..");
    }

    #endregion

    #region functions
    bool IsInRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
            return true;
        else
            return false;
    }

    public void DEAD()
    {
        StopAllCoroutines();
        agent.updateRotation = false;
        currentState = States.DIE;
        StartCoroutine(DIE());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, slowRange);
    }
    
    #endregion
}
