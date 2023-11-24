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
    [Header ("Variables")]
    public float sightRange;
    public float attackRange;
    public float moveSpeed;

    [Header ("Objects")]
    public GameObject player;

    [Header("Territory")]
    public Transform spawnPoint;
    public float chaseRange; //the area this object can follow player

    private NavMeshAgent agent;

    [HideInInspector] public Color sightColor;
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
        this.transform.position = spawnPoint.position; //spawn on spawnpoint
    }
    #endregion

    #region FiniteStateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(EnemyFSM());
        }
    }
    #endregion

    #region Coroutines
    IEnumerator IDLE() //transitions to Chasing
    {
        while (currentState == States.IDLE)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightRange)
            {
                currentState = States.CHASING;
                yield return StartCoroutine(currentState.ToString());
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CHASING() //transitions to return or attacking
    {
        //move to player
        if(Vector3.Distance(transform.position, player.transform.position)  < attackRange) //can attack
        {
            currentState = States.ATTACKING;
            yield return StartCoroutine(currentState.ToString());
        }


    }

    IEnumerator ATTACKING()
    {
        //attack delay before explosion
        //destroy self
        yield return null;
    }

    IEnumerator RETURN()
    {
        yield return StartCoroutine(currentState.ToString());
    }
    #endregion

}