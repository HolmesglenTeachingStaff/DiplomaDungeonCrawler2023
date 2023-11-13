using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public Transform player;
    private NavMeshAgent agent;
    public Color sightColor;
    #endregion

    #region States
    /// Declare states. If you add a new state remember to add a new States enum for it. 
    /// These states are what we use to change the finiate state machine (FSM) coroutine between its different states.
    public enum States { IDLE, ROAMING, CHASING, ATTACKING }

    //this variable holds ONE of the states, as it can only be one at once. Switches between them as this variable changes.
    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        //starts in idle. It has conditions to switch to the others.
        currentState = States.IDLE;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //start the fsm, it's never turned off/always on. Initiates the changes between the corroutiens. 
        StartCoroutine(EnemyFSM());
    }
    #endregion

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        //while the coroutine is running (while im pretty sure it always)
        while (true)
        {
            //Check what STRING is specifically in the currentState Variable, then find a coroutine with that same name. thats why the variable is literally just holing a string of the enum name
            // and is why you must be sure TO NAME THE COROUTINES THE EXACT SAME AS THE ENUMS
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behaviour Coroutines-
    IEnumerator IDLE()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour. EG: turning on/off any bools to cause any initial animations/effects/sounds to play, that should only be played once.

        Debug.Log("Alright, seems no evil Player is around, I can chill!");

        //UPDATE IDLE STATE >
        //put any code here you want to repeat in idle. This while loop repeats for as long as the state stays on idle. 
        while (currentState == States.IDLE)
        {
            //check player distance
            if (Vector3.Distance(transform.position, player.position) < sightRange)
            {
                //change state if (in this case) the distance to player is less than sight range
                currentState = States.CHASING;
            }

            //run through above once, then wait for (in this case) 1 frame before running it again.
            yield return new WaitForSeconds(0.5f);
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
    #endregion

    //used to see the "sight range" for debugging, to make sure its not the sight range causing issues
    void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }


    /// For a "cone" of vision: Still have a vision 'raduius', draw two lines using X number for angle 'draw a line -X degrees from where this is facing, and another at +X degrees'. 
    /// use those lines to 'cut out' a cone from your vision radius. Then when a target is detected in vision radius, just check if it's angle from the enemy is 
    /// larger than right degree away or less than left degrees away. 
    // THERE IS A SAMPLE ONE IN LAST YEARS DUNGEON CRAWLER. Think maybe in resources SESSION 7? Use it for Jorogumo. 


}
