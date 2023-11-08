using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DR_Elemental_StateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    private DR_ElementalPlayerTracker tracker;
    private OrbitTarget orbitTarget;
    private Animator anim;
    public float lastCharge, timeBetweenCharges, chargeTime;
    [HideInInspector]
    public Color sightColor;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { PUDDLE, EMERGING, FOLLOW, AIMING, ATTACKING, OVERHEATING }

    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.PUDDLE;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        tracker = GetComponentInChildren<DR_ElementalPlayerTracker>();
        orbitTarget = GetComponent<OrbitTarget>();
        orbitTarget.orbitTarget = player.gameObject;
        anim = GetComponentInChildren<Animator>();
        //start the fsm
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

    #region Behaviour Coroutines
    IEnumerator PUDDLE()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PUDDLE)
        {
            if (tracker.playerEntered) currentState = States.EMERGING;
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left


    }
    IEnumerator EMERGING()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour
        anim.SetBool("Emerge", true);

        yield return new WaitForSeconds(5f);

        currentState = States.FOLLOW;
        


    }
    IEnumerator FOLLOW()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        lastCharge = Time.time;
        float originalSpeed = agent.speed;
        agent.speed *= 0.25f;


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.FOLLOW)
        {
            if (Vector3.Distance(player.position, transform.position) <= orbitTarget.orbitDistance)
            {
                agent.speed = originalSpeed * 2f;
            }
            agent.SetDestination(orbitTarget.orbitPosition);

            anim.SetFloat("Velocity", agent.velocity.magnitude);
            yield return new WaitForEndOfFrame();
            if(Time.time - lastCharge > timeBetweenCharges)
            {
                currentState = States.AIMING;
            }
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator AIMING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");
        anim.SetBool("Charging", true);
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        float timer = 0;

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.AIMING)
        {
            timer += Time.deltaTime;
            if(timer >= chargeTime)
            {
                currentState = States.ATTACKING;
                anim.SetBool("Charging", false);
            }

            //rotate to face player
            Vector3 dir = player.position - transform.position;
            Quaternion desiredRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, 0.02f);
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator ATTACKING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.AIMING)
        {
          
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
