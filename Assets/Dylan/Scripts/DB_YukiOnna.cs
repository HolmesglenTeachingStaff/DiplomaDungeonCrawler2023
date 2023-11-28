using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DB_YukiOnna : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float slowRange;
    public float attackRange;

    public Transform player;
    NavMeshAgent agent;
    private Animator anim;
    public float lastAttack, timeBetweenAttacks;
    private Stats stats;

    public List<Vector3> waypoints = new List<Vector3>();
    private int waypointIndex;
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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats.OnDamaged.Invoke();

        StartCoroutine(EnemyFSM());
    }
    #endregion

    #region FSM
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Coroutines
    IEnumerator IDLE()
    {
        float timer = 0; //create a timer to wait 5 seconds;

        while (currentState == States.IDLE)
        {
            if (IsInRange(sightRange))
            {
                currentState = States.CHASING;
            }
            //increase the timer
            timer ++;
            yield return new WaitForSeconds(1);
            //check if it should patrol
            if (timer > 5)
            {
                currentState = States.PATROLLING;
            }
        }
    }
    IEnumerator CHASING()
    {
        agent.updateRotation = true;
        
        while(currentState == States.CHASING)
        {
            agent.SetDestination(player.position);
            if (IsInRange(attackRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.ATTACKING;
            else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            else agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator PATROLLING()
    {
        agent.SetDestination(waypoints[waypointIndex]);
        Debug.Log("Slow patrol");

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
    }

    IEnumerator ATTACKING()
    {
        while (currentState == States.ATTACKING)
        {
            if (IsInRange(attackRange))
            {
                anim.Play("IceSpell");
                yield return new WaitForSeconds(3f);
            }
            else
            {
                currentState = States.CHASING;
            }
            if(stats.currentHealth <= 0)
            {
                currentState = States.DIE;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DIE()
    {
        anim.Play("Death");
        yield return new WaitForSeconds(2);

        while (currentState == States.DIE)
        {
            yield return new WaitForSeconds(1.5f);
            if (stats.currentHealth <= 0)
            {
                Destroy(gameObject);
                stats.Die();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Functions
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, slowRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion
}
