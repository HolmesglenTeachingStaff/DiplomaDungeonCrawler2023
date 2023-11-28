using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MJ_CorruptPriestBehavior : MonoBehaviour
{
    /// <summary>
    /// FiniteStateMachine for CorruptPriest
    /// </summary>
    
    #region variables
    [Header ("Variables")]
    public float sightRange;
    public float attackRange;
    public float moveSpeed;
    public float attackInterval;

    [Header ("Objects")]
    public GameObject player;
    public Transform[] projectileSpawns; //spawnpoints for projectiles

    [Header ("Territory")]
    public Transform spawnPoint;
    public float chaseRange; //the area this object can follow player

    private NavMeshAgent agent;

    [HideInInspector] public Color sightColor;

    [SerializeField] GameObject projectile;
    #endregion

    #region States
    public enum States {IDLE, CHASING, ATTACKING, RETURN};
    public States currentState;

    private void Awake()
    {
        currentState = States.IDLE;

    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CHASING() //transitions to ATTACK or RETURN
    {
        Debug.Log(this.gameObject.name + ": Chasing player!");

        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.transform.position);
            //walk to destination
            //play Walking animation

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) //can attack
            {
                currentState = States.ATTACKING;
                yield return StartCoroutine(currentState.ToString());
            }
            else if (Vector3.Distance(transform.position, spawnPoint.position) > chaseRange) //far from spawn
            {
                Debug.Log(this.gameObject.name + ": Too far from my spot!");
                currentState = States.RETURN;
                yield return StartCoroutine(currentState.ToString());
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > sightRange) //out of sight
            {
                Debug.Log(this.gameObject.name + ": Lost the player!");
                currentState = States.RETURN;
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

        for (int i = 0; i < projectileSpawns.Length; i++)  //summon projectiles with interval
        {
            Instantiate(projectile, projectileSpawns[Random.Range(0, projectileSpawns.Length - 1)]);
            new WaitForSeconds(attackInterval);
        }
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator RETURN()
    {
        agent.SetDestination(spawnPoint.position);
        //walk to spawnpoint
        
        //heal rapidly to max HP while returning
        

        currentState = States.IDLE;
        yield return StartCoroutine(currentState.ToString());
    }
    #endregion
}