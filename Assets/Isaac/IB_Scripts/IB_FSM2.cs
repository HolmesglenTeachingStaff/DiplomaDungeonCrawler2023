using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IB_FSM2 : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    public NavMeshAgent agent;
    public Color sightColor;

    public IB_DamageReactions KnockBack;
    public GameObject tenguM;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public GameObject Weapon;

    private Animator anim;
    #endregion

    #region States
    //Declare states. If you add a new sate to your character, remember to add a new States enum for it.
    public enum States { IDLE, ROAMING, CHASING, ATTACKING, DEATH }
    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.IDLE;
    }
    #endregion

    void Start()
    {
        KnockBack = GetComponent<IB_DamageReactions>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Weapon.GetComponent<BoxCollider>().enabled = false;
        //start the fsm
        StartCoroutine(EnemyFSM());
    }

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
        //ENTER THE IDLE STATE > put any code here that you want to run at the start of the behavior
        Debug.Log("Alright, seems no evil Player is around, I can chill!");

        //Update IDLE STATE > put any code here to repeat during the state being active
        while (currentState == States.IDLE)
        {
            //Check player distance
            if (Vector3.Distance(transform.position, player.position) < sightRange)
            {
                //Change state
                currentState = States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE > write any code here you want to run when the state is left

        Debug.Log("oh no I see the player!");
    }
    IEnumerator CHASING()
    {
        //ENTER THE CHASING STATE > put any code here that you want to run at the start of the behavior
        Debug.Log("I'ma Get Ya!");

        //Update IDLE STATE > put any code here to repeat during the state being active
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
            }

           
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE > write any code here you want to run when the state is left

        Debug.Log("oh no I see the player!");
    }
    IEnumerator ATTACKING()
    {
        anim.Play("Attack");
        Weapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        Weapon.GetComponent<BoxCollider>().enabled = false;
        currentState = States.CHASING;
    }

    IEnumerator DEATH()
    {
        while (currentState == States.DEATH)
        {
            anim.Play("Death");
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    public void Death()
    {
        currentState = States.DEATH;
    }
}
