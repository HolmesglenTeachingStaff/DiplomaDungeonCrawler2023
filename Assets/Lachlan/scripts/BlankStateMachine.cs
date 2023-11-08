using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class BlankStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    public GameObject weapon;

    [HideInInspector]
    public Color sightColor;
    #endregion
   
    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { PATROLLING, CHASING, ATTACKING }

    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //start the fsm
        StartCoroutine(EnemyFSM());
        weapon.GetComponent<BoxCollider>().enabled = false;
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
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator PATROLLING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {

            yield return new WaitForEndOfFrame();
        }
        IEnumerator ATTACKING()
        {
            //ENTER THE Chasing STATE >
            //put any code here that you want to run at the start of the behaviour

            Debug.Log("ATTACKING");

            //UPDATE Chasing STATE >
            //put any code here you want to repeat during the state being active
            while (currentState == States.ATTACKING)
            {

                weapon.GetComponent<BoxCollider>().enabled = true;

                yield return new WaitForEndOfFrame();

                weapon.GetComponent<BoxCollider>().enabled = false;

            }

            //EXIT IDLE STATE >
            //write any code here you want to run when the state is left

            Debug.Log("Oh no I see the player!");
        }
        #endregion

        void OnDrawGizmosSelected()
        {
            print("Drawing Gizmo's");
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }
    }
}