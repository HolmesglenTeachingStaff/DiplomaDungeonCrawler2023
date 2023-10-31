using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MJ_FiniteStates : MonoBehaviour
{
    /// <summary>
    /// FiniteStateMachine for NPCs
    /// </summary>

    #region variables
    public float sightRange;
    public float attackRange;
    public Transform player;
    private NavMeshAgent agent;

    [HideInInspector] public Color sightColor;
    #endregion

    #region States
    public enum States {IDLE, CHASING, ATTACKING };
    public States currentState;

    private void Awake()
    {
        currentState = States.IDLE;
    }
    void Start()
    {
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
        //put any code here that you want to run at the start of the behaviour

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active

        while (currentState == States.IDLE)
        {
            yield return new WaitForSeconds(1);
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

            yield return new WaitForEndOfFrame();
        }

        //EXIT CHASING STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Lost the player");
    }

    IEnumerator ATTACKING()
    {

        //ENTER THE ATTACKING STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE ATTACKING STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {

            yield return new WaitForEndOfFrame();
        }

        //EXIT ATTACKING STATE >
        //write any code here you want to run when the state is left

    }
    #endregion
}