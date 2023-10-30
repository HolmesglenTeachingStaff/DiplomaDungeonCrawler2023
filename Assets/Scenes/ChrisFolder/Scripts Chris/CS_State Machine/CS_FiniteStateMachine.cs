using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_FiniteStateMachine : MonoBehaviour
{

    #region variables

    public float sightRange;
    public float meleeRange;
    //public bool idleOvertime;
    //public float roamTime = 5f; //trying to create ROAMING loop ^^

    public Transform player;
    private NavMeshAgent agent;

    public Color sightColour; //i could add my own destint colour this way. To do so, below code would be "Gizmos.color = sightColour"

    //patrol settings
    [SerializeField] Transform[] nodes;
    int currentNode;

    private Animator anim;

    #endregion

    #region States

    //declare states, add states where needed on the enum so it's accissible by the character
    public enum States {IDLE, ROAMING, ATTACKING, CHASING, PATROLLING, CHARGING, STUNNED}
    public States currentState;

    #endregion


    #region intialization

    //set default state
    private void Awake()
    {
        //idleOvertime = false; trying to create ROAMING loop after so many IDLE states
        currentState = States.IDLE;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        //start the FSM (Finite State Machine)
        StartCoroutine(EnemyFSM());
    }

    #endregion



    #region Finite StateMachine

    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString()); //this just puts the coroutines to string rather than always writing "IDLE"
        }
        
    }

    #endregion

    #region Behaviour Coroutines

    IEnumerator IDLE()
    {
        //enter the IDLE state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("Ok, no one here, lets chill");

        //UPDATE IDLE state
        //put any code here you want to repeat during the state being active
        while(currentState == States.IDLE)
        {   

            //NEED TO IMPLEMENT COUNTER BEFORE GOING INTO PATROLLING BELOW
            yield return new WaitForSeconds(2);

            currentState = States.PATROLLING;

           /* for(int i = 0; i < roamTime.Length; i ++) //trying to create roaming loop
            {
                if(i =>4)
                {
                    idleOvertime = true;

                    if(idleOvertime = true)
                    {
                        currentState = States.ROAMING;
                    }
                    
                }
            }*/

            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                currentState = States.CHASING;
            }

            //roamTime += 1 (); //trying to create ROAMING loop
            yield return new WaitForEndOfFrame();
        }

        //exit IDLE state
        //write any code here you want to run when the state is left
        Debug.Log("AN INTRUDER!");

    }

    IEnumerator PATROLLING()
    {
        //ENTER THE PATROLLING STATE >
        //put any code here that you want to run while patrolling

       Debug.Log("I'll find you");

       agent.SetDestination(nodes[currentNode].position);

        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during patrolling state being active
        while (currentState == States.PATROLLING)
        {

            if (IsInRange(sightRange)) currentState = States.CHASING; //comparing range, making it true or false depending with function IsInRange below

             if(Vector3.Distance(player.position, transform.position) <sightRange) 
             {
                currentState = States.CHASING;
             }

            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//pathPending is when the agent is still in the process of picking a path
            {
                yield return new WaitForSeconds(Random.Range(0, 5)); //randomizes the time character waits between nodes
                agent.speed = Random.Range(3, 8); //randomizes the speed the character moves between nodes

                //currentNode = Random.Range(0, nodes.Length); //this would randomize the nodes the npc goes between

                currentNode = (currentNode + 1) % nodes.Length; //this divides the first number by the second number, starting from node 0 going upward, resseting the nodes to 0 when they're completed through.

                //currentNode++; //increasing node count
                agent.SetDestination(nodes[currentNode].position);
            }

            yield return new WaitForEndOfFrame();
        }

        //EXIT PATROLLING STATE >
        //write any code here you want to run when the state is left

        
    }

   
    IEnumerator CHASING()
    {
        //enter the CHASING state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("GIT EM!");

        //UPDATE CHASING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.CHASING)
        {
            //checking if state should change based off distance of player using the below function (IsInRange())
            if(!IsInRange(sightRange)) currentState = States.IDLE;
            else if (IsInRange(meleeRange)) currentState = States.ATTACKING;

            agent.SetDestination(player.position);

            if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("Where'd they go?");
            }
            else if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }

            yield return new WaitForEndOfFrame();
        }

        //exit CHASING state
        //write any code here you want to run when the state is left
        Debug.Log("Ah, must have been the wind.");

    }


    IEnumerator ATTACKING()
    {
        //enter the ATTACKING state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("GOT YA!");
        agent.SetDestination(player.position);
        
        //UPDATE ATTACKING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.ATTACKING)
        {
            

            //add ATTACK anim here
            anim.Play("Standing_Attack");
            yield return new WaitForSeconds(2f); //the number is the time length of the animation

            //currentState = States.CHASING;
            //yield return null;

            //DoDamage(); //this will need to be declared on another script as you can't create a function within a coroutine

            
            /*if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                Debug.Log("Where'd they go?");
                currentState = States.IDLE;
                
            }
            else if(Vector3.Distance(transform.position, player.position)> meleeRange && Vector3.Distance(transform.position, player.position) < sightRange)
            {
                Debug.Log("Damnit!");
                currentState = States.CHASING;
                
            }
            else if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }*/

            if (!IsInRange(meleeRange)) //much shorter version of the above code
            {
                if (IsInRange(sightRange)) currentState = States.CHASING;
                else currentState = States.IDLE;
            }

            yield return new WaitForEndOfFrame();
        }

        //exit ATTACKING state
        //write any code here you want to run when the state is left
        Debug.Log("Ah, must have been the wind.");

    }


 IEnumerator CHARGING()
    {
        //ENTER THE PATROLLING STATE >
        //put any code here that you want to run while patrolling

       Debug.Log("I'll find you");

       agent.SetDestination(nodes[currentNode].position);

        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during patrolling state being active
        while (currentState == States.PATROLLING)
        {

            if (IsInRange(sightRange)) currentState = States.CHASING; //comparing range, making it true or false depending with function IsInRange below

             if(Vector3.Distance(player.position, transform.position) <sightRange) 
             {
                currentState = States.CHASING;
             }

            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//pathPending is when the agent is still in the process of picking a path
            {
                yield return new WaitForSeconds(Random.Range(0, 5)); //randomizes the time character waits between nodes
                agent.speed = Random.Range(3, 8); //randomizes the speed the character moves between nodes

                //currentNode = Random.Range(0, nodes.Length); //this would randomize the nodes the npc goes between

                currentNode = (currentNode + 1) % nodes.Length; //this divides the first number by the second number, starting from node 0 going upward, resseting the nodes to 0 when they're completed through.

                //currentNode++; //increasing node count
                agent.SetDestination(nodes[currentNode].position);
            }

            yield return new WaitForEndOfFrame();
        }

        //EXIT PATROLLING STATE >
        //write any code here you want to run when the state is left

        
    }


    IEnumerator STUNNED()
    {
        //enter the STUNNED state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("OOUUFF");

        //UPDATE STUNNED state
        //put any code here you want to repeat during the state being active

        //add stunned animation here

        yield return new WaitForSeconds(3f);

        while(currentState == States.STUNNED)
        {
            
            //checking if state should change based off distance of player using the below function (IsInRange())
            if(!IsInRange(sightRange)) currentState = States.IDLE;
            else if (IsInRange(meleeRange)) currentState = States.ATTACKING;

            agent.SetDestination(player.position);

            if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("Where'd they go?");
            }
            else if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }

            yield return new WaitForEndOfFrame();
        }

        //exit CHASING state
        //write any code here you want to run when the state is left
        Debug.Log("Ah, must have been the wind.");

    }



    #endregion



    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta; // setting colour to see the difference between similar Gizmos. Color is a class holding different colours within it.
        Gizmos.DrawWireSphere(transform.position, sightRange); //Gizmos is a class itself, this line of code allows us to see the sightRange or meleeRange above in the variables

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }


    #endregion

    
   
    public void DoDamage() //call this from another script as functions can't be made within Coroutines
    {
        Debug.Log("En Garde!");

        
    }


    bool IsInRange(float range) //easy way to compare ranges
    {
        if (Vector3.Distance(player.position, transform.position) <range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
