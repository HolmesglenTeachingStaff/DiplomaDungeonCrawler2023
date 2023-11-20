using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// State machine for a Summoner, used to opperate and switch between states.
/// Will be used for the majority of managing a Summoners movements/actions.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BW_SummonerStateMachine : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    private Animator anim;
    private OrbitTarget orbitTarget;
    private Stats stats;

    private StatSlider statSlider;

    public Transform player;
    public Transform summonLocation;

    public GameObject objectToSummon;

    public bool checkingForPlayer = true;

    [Header("Reaction Range Values")]
    public float sightRange = 12;
    public float meleeRange = 2;

    //Nodes to indicate where to patrol.
    [SerializeField] Transform[] nodes;
    int currentNode;
    #endregion

    //The behaviour states available for a Summoner to switch between.
    #region States
    public enum States {IDLE, PATROLLING, MELEE, COMBAT}

    public States currentState;
    #endregion

    //Starting all required components.
    #region Initialization
    private void Awake()
    {
        currentState = States.IDLE;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        orbitTarget = GetComponent<OrbitTarget>();
        orbitTarget.orbitTarget = player.gameObject;
        stats = GetComponent<Stats>();

        StartCoroutine(SummonerFSM());

        //statSlider.UpdateSlider();
    }
    #endregion

    //Initialize the state machine.
    #region Finite State Machine
    IEnumerator SummonerFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Update
    void Update()
    {
        //Chase the player, if seen from any state
        if (checkingForPlayer == true && WithinRange(sightRange))
        {
            currentState = States.COMBAT;
        }

        //Change to MELEE state from any state if the player gets too close
        if (WithinRange(meleeRange))
        {
            currentState = States.MELEE;
        }

        //Rotate to face the player while in sightRange
        if (WithinRange(sightRange))
        {
            transform.LookAt(player);
        }
    }
    #endregion

    //Contents of the states, and how to change between them.
    #region Behaviour Coroutines


    #region IDLE
    IEnumerator IDLE()
    {
        float timer;
        timer = 0;

        checkingForPlayer = true;

        while (currentState == States.IDLE)
        {
            timer += Time.deltaTime;

            //play IDLE animation

            //Time spent remaining IDLE
            if (timer >= 6)
            {
                currentState = States.PATROLLING;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region PATROLLING
    IEnumerator PATROLLING()
    {
        checkingForPlayer = true;

        agent.SetDestination(nodes[currentNode].position);
        currentNode = Random.Range(0, nodes.Length);

        while (currentState == States.PATROLLING)
        {
            //play movement animation

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = States.IDLE;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region COMBAT
    IEnumerator COMBAT()
    {
        checkingForPlayer = false;

        int maxSummons = 0;
        float summonTimer = 0;

        while (currentState == States.COMBAT)
        {
            if (!WithinRange(sightRange)) currentState = States.IDLE;

            //play movement animation

            summonTimer += Time.deltaTime;
            //Spawn a new summon every 10 seconds (With a max of 3 summons at one time)
            if (maxSummons < 3 && summonTimer >= 10)
            {
                Instantiate(objectToSummon, summonLocation.position, summonLocation.rotation);
                summonTimer = 0;
                maxSummons++;
            }

            //Follows the player while maintaining distance
            agent.SetDestination(orbitTarget.orbitPosition);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion

    #region MELEE
    IEnumerator MELEE()
    {
        checkingForPlayer = false;

        agent.SetDestination(agent.transform.position);

        //put any code here you want to repeat during the state being active
        while (currentState == States.MELEE)
        {
            if (!WithinRange(meleeRange)) currentState = States.COMBAT;
            //play animation
            //set active the damage collider

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    #endregion


    #endregion

    //Function called from the inspector using the OnDeath Event in the Stats script
    public void Dead()
    {
        //play death animation
        //wait until it's finished
        Destroy(gameObject);
    }


    //Used to help visualize the range values in scene.
    #region Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, meleeRange);
    }
    #endregion

    //Determine whether the player is within a certain range
    #region Range
    bool WithinRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
