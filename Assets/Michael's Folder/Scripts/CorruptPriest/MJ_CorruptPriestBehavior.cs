using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MJ_CorruptPriestBehavior : MonoBehaviour
{
    /// <summary>
    /// FiniteStateMachine for CorruptPriest
    /// </summary>

    #region variables
    [Header("Variables")]
    public float sightRange;
    public float attackRange;
    public float moveSpeed;
    public float attackInterval;

    [Header("Objects")]
    public GameObject player;
    public Transform[] projectileSpawns; //spawnpoints for projectiles\

    [SerializeField]Animation anim;

    [Header("Territory")]
    public Transform spawnPoint;
    public float chaseRange; //the area this object can follow player

    private NavMeshAgent agent;


    [Header("Gizmos")]
    public Color sightColor;
    public Color attackColor;

    [SerializeField] GameObject projectile;
    #endregion

    #region States
    public enum States { IDLE, CHASING, ATTACKING, RETURN };
    public States currentState;

    private void Awake()
    {
        currentState = States.IDLE;

    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        this.transform.position = spawnPoint.position;
        StartCoroutine(EnemyFSM()); //start the StateMachine
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

    #region Behavior Coroutines
    IEnumerator IDLE() //transitions to CHASING
    {
        //play IDLE animation

        while (currentState == States.IDLE)
        {
            //if attacked, chase player

            if (Vector3.Distance(transform.position, player.transform.position) < sightRange) //sees player
            {
                currentState = States.CHASING;
                yield return StartCoroutine("CHASING");
            }
           
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CHASING() //transitions to ATTACK or RETURN
    {
        Debug.Log(this.gameObject.name + ": Chasing player!");

        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.transform.position);

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) //can attack
            {
                currentState = States.ATTACKING;
                yield return StartCoroutine(currentState.ToString());
            }
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator ATTACKING()
    {
        agent.SetDestination(transform.position);
        Debug.Log(this.gameObject.name + ": Dark orbs!");

        //play attack animation
        while (currentState == States.ATTACKING & !projectile)
        {
            for (int i = 0; i < projectileSpawns.Length;)  //summon projectiles with interval
            {
                Instantiate(projectile, projectileSpawns[Random.Range(0, projectileSpawns.Length - 1)]);
                new WaitForSeconds(attackInterval);
            }
        }
        CheckPlayer();
        yield return StartCoroutine(currentState.ToString());
    }
    IEnumerator RETURN() //continuously return to spawnpoint while healing to max HP
    {
        agent.SetDestination(spawnPoint.position);

        //walk to spawnpoint

        //heal rapidly to max HP while returning
        while (currentState == States.RETURN)
        {
            if (agent.destination == transform.position)
            {
                currentState = States.IDLE;
                yield return StartCoroutine(currentState.ToString());
            }
        }
        
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region methods and Gizmos
    void CheckPlayer() //determines next Behavior
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) //can attack
        {
            currentState = States.ATTACKING;
        }
        else if (Vector3.Distance(transform.position, spawnPoint.position) > chaseRange) //far from spawn
        {
            Debug.Log(this.gameObject.name + ": Too far from my spot!");
            currentState = States.RETURN;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > sightRange) //out of sight
        {
            Debug.Log(this.gameObject.name + ": Lost the player!");
            currentState = States.RETURN;

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = attackColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion
}