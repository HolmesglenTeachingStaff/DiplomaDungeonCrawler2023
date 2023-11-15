using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_FiniteStateMachine : MonoBehaviour
{

    #region variables

    public float sightRange;
    public float meleeRange;
    public float chargeRange;
    public bool idleDone;
    public float[] idleTimer;
    //public float damageAmount;
    public float damageRange;

    //public CS_DamageReactions.DamageBurst damageBurst;
    //public Stats stats;
    //public CS_DamageReactions.Die die;
    //public StatSystem.DamageType damageType;
    //public LayerMask attackableLayers;

    //public bool idleOvertime;
    

    public bool chargePointer;

    
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
    public enum States {IDLE, ATTACKING, CHASING, PATROLLING, CHARGING, STUNNED}
    public States currentState;

    #endregion


    #region intialization

    //set default state
    private void Awake()
    {
        meleeRange = damageRange;
        //Stats stats = enemy.GetComponent<Stats>();

        idleDone = false;
        
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
        agent.speed = 3;
        float timer = 0;

        //add IDLE anim
        while(currentState == States.IDLE)
        {   

            //checking if we need to chase the player
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                currentState = States.CHASING;
            }  
            else if (timer >4) //check if we should patrol
            {
                currentState = States.PATROLLING;
            }

            //if niether is true, we are still in IDLE and the while loop will continue, so we increase our timer and repeat
            timer += Time.deltaTime;

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
        agent.speed = 3;
        agent.SetDestination(nodes[currentNode].position);

        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during patrolling state being active
        while (currentState == States.PATROLLING)
        {

            if (IsInRange(sightRange)) currentState = States.CHASING; //comparing range, making it true or false depending with function IsInRange below

             if(Vector3.Distance(transform.position, player.position) <sightRange) 
             {
                currentState = States.CHASING;
             }

            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//pathPending is when the agent is still in the process of picking a path
            {
                yield return new WaitForSeconds(Random.Range(0, 5)); //randomizes the time character waits between nodes
                agent.speed = Random.Range(1, 4); //randomizes the speed the character moves between nodes

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
        agent.speed = 3;
        //UPDATE CHASING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.CHASING)
        {
            //checking if state should change based off distance of player using the below function (IsInRange())
            if(!IsInRange(sightRange)) currentState = States.IDLE;
            else if (IsInRange(meleeRange)) currentState = States.ATTACKING;

            if(Vector3.Distance(transform.position, player.position) == chargeRange)
            {
                currentState = States.CHARGING;
            }

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
        agent.speed = 3;
        agent.SetDestination(player.position);

        //CS_DamageReactions.DamageBurst (damageBurst); //accessing CS_DamageReactions script, then applying DamageBurst function
        
        //UPDATE ATTACKING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.ATTACKING)
        {
                    
            //add ATTACK anim here
            // anim.Play("Standing_Attack");
            yield return new WaitForSeconds(2f); //the number is the time length of the animation
            Debug.Log("attack AGANE!");

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

            /* if (!IsInRange(meleeRange)) //much shorter version of the above code
            {
                if (IsInRange(sightRange)) currentState = States.CHASING;
                else currentState = States.IDLE;
            } */

            if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }
             else if(Vector3.Distance(transform.position, player.position) == chargeRange)
            {
                currentState = States.CHARGING;
            }
            else if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                currentState = States.CHASING;
            }           
            else if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("Where'd they go?");
            }
            
            yield return new WaitForEndOfFrame();
        }

        //exit ATTACKING state
        //write any code here you want to run when the state is left
        Debug.Log("Ah, must have been the wind.");

    }


 IEnumerator CHARGING()
    {
        //ENTER THE CHARGING STATE >
        //put any code here that you want to run while CHARGING

         Debug.Log("HERE I COME!");
         agent.speed = 3;
         //CHARGING anim here
         //charge here
         //do damage on hit?

         //if charging misses check for on trigger enter wall, then enter STUNNED state

         //UPDATE CHARGING STATE

         //put any code here you want to repeat during CHARGING state being active
        while (currentState == States.CHARGING)
        {

            if (IsInRange(sightRange)) currentState = States.CHASING; //comparing range, making it true or false depending with function IsInRange below

             if(Vector3.Distance(transform.position, player.position) <sightRange) 
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
        //make damge vulnerable

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
            if(Vector3.Distance(transform.position, player.position)< sightRange)
            {
                currentState = States.CHASING;
            
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chargeRange);
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
