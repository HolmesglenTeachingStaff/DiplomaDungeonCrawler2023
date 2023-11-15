using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class JP_BlankStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    private float timer=0;
    private Vector3 target;
    private Vector3 home;
    private Animator ani;
    private bool areaAni=false;
    private bool isdead=false;
    private bool amAttacking=false;

    [HideInInspector]
    public Color sightColor;
    #endregion

   
    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, PATROLLING, CHASING, ATTACKING, DIYING}

    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponentInChildren<Animator>();
        //start the fsm
        StartCoroutine(EnemyFSM());
        home=transform.position;
    }
    void Update()
    {
        ani.SetBool("isinarea",areaAni);
        ani.SetBool("amdead",isdead);
        ani.SetBool("isatacking",amAttacking);
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
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour

        areaAni=false;
        yield return new WaitForSeconds(ani.speed);

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            if(sightRange>Vector3.Distance(player.position,transform.position))
            {
                currentState=States.CHASING;
            }
            yield return new WaitForSeconds(0.5f);
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        
    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        
        areaAni=true;
        yield return new WaitForSeconds(ani.speed);

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            if(sightRange<Vector3.Distance(player.position,transform.position))
            {
                currentState=States.IDLE;
            }
            Vector3 facingDir = player.position - transform.position;
            transform.rotation = Quaternion.LookRotation(facingDir);

            timer=timer+1.0f;
            if(timer==600f)
            {
                target=player.position;
                timer=0f;
                yield return new WaitForSeconds(0.5f);
                currentState=States.ATTACKING;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator ATTACKING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        amAttacking=true;
        agent.speed=50;
        agent.SetDestination(target);
        agent.updateRotation=false;
        yield return new WaitForSeconds(1);
        agent.SetDestination(home);

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.ATTACKING)
        {
            if(!agent.pathPending && agent.remainingDistance == agent.stoppingDistance)
            {
                agent.updateRotation=true;
                currentState=States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
        amAttacking=false;
    }
    IEnumerator DIYING()
    {
        currentState=States.DIYING;
        isdead=true;
        yield return new WaitForEndOfFrame();
        isdead=false;
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        yield return new WaitForEndOfFrame();
    }

    public void DieActive()
    {
      StartCoroutine(DIYING());
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

}