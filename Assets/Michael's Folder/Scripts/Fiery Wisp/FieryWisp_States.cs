using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieryWisp_States : MonoBehaviour
{
    #region variables
    public float sightRange;

    public float attackRange;
    public Transform player;

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


}