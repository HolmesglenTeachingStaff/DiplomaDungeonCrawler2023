using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MJ_CorruptPriestBehavior : MonoBehaviour
{
    /// <summary>
    /// FiniteStateMachine for NPCs
    /// </summary>

    // 0=idle, 1=chasing, 2=attack, 3=death
    #region variables
    public float sightRange;
    public float attackRange;
    public Transform player;

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
        this.transform.position = spawnPoint.position;
        projectile = GetComponent<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        //start the StateMachine
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

    #region Behavior Coroutines
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >
        //if position is not in spawnpos, set movetarget to spawnpos

        while (currentState == States.IDLE)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightRange)
            {
                currentState = States.CHASING;
                yield return StartCoroutine("CHASING");
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
    }

    IEnumerator CHASING()
    {

        //ENTER THE CHASING STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("Chasing player");

        //UPDATE CHASING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > sightRange) //out of sight
            {
                currentState = States.IDLE;
                Debug.Log("Lost the player");
                yield return StartCoroutine("IDLE");
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange) //can attack
            {
                currentState = States.ATTACKING;
                yield return StartCoroutine("ATTACKING");
            }
            else if (Vector3.Distance(transform.position, spawnPoint.position) > chaseRange) //far from spawn
            {
                currentState = States.IDLE;
                Debug.Log(this.gameObject.name + ": Too far from my spot!");
                yield return StartCoroutine("IDLE");

            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ATTACKING()
    {

        //ENTER THE ATTACKING STATE >
        //play animation
        //summon (instantiate) projectiles with delay


        //UPDATE ATTACKING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {

            yield return new WaitForSeconds(.5f);
        }

        //EXIT ATTACKING STATE >
        //write any code here you want to run when the state is left

    }
    #endregion
}