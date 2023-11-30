using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class RD_AmaterasuFiniteStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float rangedRange;
    //variables for attack timers;
    public float timeBetweenAttacks;
    float lastAttack;

    public Transform player;
    private NavMeshAgent agent;

    private Animator anim;
    public Collider weapon1;

    //patrolling
    public List<Vector3> waypoints = new List<Vector3>();
    private int waypointIndex;


    [HideInInspector]
    public Color sightColor;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { IDLE, CHASING, PATROLLING, HEALING, TALKING, DEATH }
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
        GetComponent<Stats>().OnDamaged.Invoke();
        agent = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();

        //turn hitboxes off by default
        weapon1.enabled = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        //collect the waypoint positions
        foreach (Transform item in transform)
        {
            if (item.tag == "Waypoint")
            {
                waypoints.Add(item.position);
            }
        }
        lastAttack = 0;
        //start the fsm
        StartCoroutine(FriendlyFSM());
    }
    #endregion

    #region Finite StateMachine
    IEnumerator FriendlyFSM()
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
        float timer = 0; //create a number to count from to track if idle should transition;

        Debug.Log("Alright, seems no evil Player is around, I can chill!");

        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.IDLE)
        {
            //check for player and count up untile idle time has run out
            if (IsInRange(rangedRange)) currentState = States.HEALING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else timer += Time.deltaTime;
            if (timer > 5) currentState = States.PATROLLING;
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator CHASING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position);

            //seek attacking the player
            if (IsInRange(rangedRange))
            {
                currentState = States.HEALING;
            }

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("aaaargh he's too fast!");
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }

    IEnumerator PATROLLING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        agent.SetDestination(waypoints[waypointIndex]);
        Debug.Log("I'ma Get Ya!");

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PATROLLING)
        {
            if (IsInRange(rangedRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.HEALING;
            else if (IsInRange(sightRange)) currentState = States.CHASING;
            else
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    waypointIndex = waypointIndex + 1 % waypoints.Count;
                }

                agent.SetDestination(waypoints[waypointIndex]);
            }

            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator HEALING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        lastAttack = Time.time;
        //roll a number to pick which attack to run
        int attackType = Random.Range(0, 100);

        if (attackType < 70)//70%chance to run the basic attack
        {
            anim.SetTrigger("waving");//run the attack animation
            weapon1.enabled = true;//enable the damage collider
            yield return new WaitForSeconds(4); //wait for the animation to end
            weapon1.enabled = false;
        }
        else if (attackType > 70)//remaining 10% chance to run the special attack
        {
            anim.SetTrigger("waving");//run the attack animation
            yield return new WaitForSeconds(1.5f); //wait for first hitbox
            //weapon2.enabled = true;//enable the damage collider
            //yield return new WaitForSeconds(0.1f); //wait for first hitbox
            //weapon3.enabled = true;

            //yield return new WaitForSeconds(0.1f); //wait for first hitbox to finish

            //weapon2.enabled = false;

            //yield return new WaitForSeconds(3); //wait for the animation to end
            //weapon3.enabled = false;
        }



        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left
        agent.updateRotation = true;
        if (IsInRange(sightRange)) currentState = States.CHASING;
        else currentState = States.PATROLLING;


        Debug.Log("Oh no I see the player!");
    }
    IEnumerator DEATH()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        weapon1.enabled = false;
        agent.speed = 0;
        agent.SetDestination(transform.position);
        anim.SetTrigger("death");
        yield return new WaitForSeconds(2f); //wait for the animation to end

        SkinnedMeshRenderer[] models = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer model in models)
        {
            model.enabled = false;
        }
        MeshRenderer[] meshs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.enabled = false;
        }
        Destroy(gameObject, 2);



        Debug.Log("Oh no I see the player!");
    }
    #endregion

    #region functions
    bool IsInRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
            return true;
        else
            return false;
    }

    public void DIE()
    {
        StopAllCoroutines();
        agent.updateRotation = false;
        currentState = States.DEATH;
        StartCoroutine(DEATH());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangedRange);
    }
    #endregion
}
