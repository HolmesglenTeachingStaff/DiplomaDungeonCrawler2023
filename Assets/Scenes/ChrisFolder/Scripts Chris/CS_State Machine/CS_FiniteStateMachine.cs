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

    public float chargeTime;

    public CS_DamageReactions damageBurst;
    public CS_DamageReactions chargeDamage;
    //public Stats stats;    
    //public StatSystem.DamageType damageType;
    //public LayerMask attackableLayers;

    //public bool idleOvertime;
    
    //charge mesh
    public GameObject chargePointer;

    
    public Transform player;
    private NavMeshAgent agent;

    public Color sightColour; //i could add my own destint colour this way. To do so, below code would be "Gizmos.color = sightColour"

        
    //shader graph addition
    public GameObject fireFeet;

    //patrol settings
    [SerializeField] Transform[] nodes;
    int currentNode;

    private Animator anim;

    #endregion

    #region States

    //declare states, add states where needed on the enum so it's accissible by the character
    public enum States {IDLE, ATTACKING, CHASING, PATROLLING, CHARGING, STUNNED, AIMING}
    public States currentState;

    #endregion


    #region intialization

    //set default state
    private void Awake()
    {
        
        //Stats stats = enemy.GetComponent<Stats>();

        damageBurst = GetComponent<CS_DamageReactions>();

        chargeDamage = GetComponent<CS_DamageReactions>();

        idleDone = false;
        
        currentState = States.IDLE;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        damageBurst.damageRange = meleeRange;

        chargePointer.SetActive(false);
        fireFeet.SetActive(false);

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
        agent.updatePosition = true;
        agent.updateRotation = true;

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
        agent.updatePosition = true;
        agent.updateRotation = true;

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

        agent.updatePosition = true;
        agent.updateRotation = true;
        //enter the CHASING state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("GIT EM!");
        agent.speed = 3;

        //lastCharge = Time.time; //this is couning up on time outside the while loop making it larger than within the while loop (TRYING YO CREATE TIMER BETWEEN CHARGES)

        //UPDATE CHASING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.CHASING)
        {
            
            transform.rotation = RotateToPlayer();

            //checking if state should change based off distance of player using the below function (IsInRange())
            if(!IsInRange(sightRange)) currentState = States.IDLE;
            if (IsInRange(meleeRange)) currentState = States.ATTACKING;

            if(Vector3.Distance(transform.position, player.position) == chargeRange)
            {
                //if(Time.time - lastCharge > timeBetweenCharges)
             //{       
                Debug.Log("Aim");         
                currentState = States.AIMING;
             //}
                
            }
            
            if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }

            if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                currentState = States.IDLE;
                Debug.Log("Where'd they go?");
            }
            
            

            agent.SetDestination(player.position);
                          

            yield return new WaitForEndOfFrame();
        }

        //exit CHASING state
        //write any code here you want to run when the state is left
        Debug.Log("Ah, must have been the wind.");

    }


    IEnumerator ATTACKING()
    {

        agent.updatePosition = true;
        agent.updateRotation = true;

        //enter the ATTACKING state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("GOT YA!");
        agent.speed = 3;
        agent.SetDestination(player.position);
        transform.rotation = RotateToPlayer();

        damageBurst.DamageBurst(); //accessing CS_DamageReactions script, then applying DamageBurst function

        //add ATTACK anim here
        // anim.Play("Standing_Attack");
        yield return new WaitForSeconds(2f); //the number is the time length of the animation
        
        //UPDATE ATTACKING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.ATTACKING)
        {
                    
            
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
            if(Vector3.Distance(transform.position, player.position) == chargeRange)
            {
                Debug.Log("Aim");
                currentState = States.AIMING;
            }
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                currentState = States.CHASING;
            }           
            if(Vector3.Distance(transform.position, player.position)> sightRange)
            {
                Debug.Log("Where'd they go?");
                currentState = States.IDLE;
                
            }
            
            yield return new WaitForEndOfFrame();
        }

        //exit ATTACKING state
        //write any code here you want to run when the state is left
        Debug.Log("restarting attack state");

    }

    IEnumerator AIMING()
    {
        //put any code here that you want to run at the start of the behaviour
        chargePointer.SetActive(true);
        fireFeet.SetActive(true);

        Debug.Log("I'ma CHARGIN MA LAZER");        
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        float aimTimer = 0;
       
        //put any code here you want to repeat during the state being active
        while (currentState == States.AIMING)
        {
            
            if(aimTimer >= chargeTime)
            {
                chargePointer.SetActive(false);
                currentState = States.CHARGING;
            }

            aimTimer += Time.deltaTime;
            //rotate to face player
            transform.rotation = RotateToPlayer(); //keeps rotating to player until we're over the aimTimer
            yield return new WaitForEndOfFrame();
        }

        //write any code here you want to run when the state is left

        Debug.Log("AIM AIM AIM");
    }


 IEnumerator CHARGING()
    {
        //ENTER THE CHARGING STATE >
        //put any code here that you want to run while CHARGING

         Debug.Log("HERE I COME!");
         agent.speed = 8;
         //CHARGING anim here

         Vector3 moveTarget = transform.position + transform.forward * 15f;
         agent.updatePosition = false;
         //attack.attackActive = true;
         RaycastHit hit;

         //charge here
         if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3f, LayerMask.NameToLayer("Environment"))) //if i get within 3 units of Environment tagged objects then do this
         {
            fireFeet.SetActive(false);
            currentState = States.STUNNED;
         }
         if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 2f, LayerMask.NameToLayer("Player"))) //if i get within 3 units of Environment tagged objects then do this
         {
            //do damage on hit?
            chargeDamage.ChargeBurst(); //accessing CS_DamageReactions script, then applying ChargeBurst function
            fireFeet.SetActive(false);    
            currentState = States.CHASING;
         }

         
         
         

         //if charging misses check for on trigger enter wall, then enter STUNNED state

         //UPDATE CHARGING STATE

         //put any code here you want to repeat during CHARGING state being active
         //float pos = 0;
         //Vector3 start = transform.position;
         //float duration = attackSpeed;
                  while (currentState == States.CHARGING)
         {
            if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {              
                fireFeet.SetActive(false); 
                currentState = States.ATTACKING;
            }
            if(Vector3.Distance(transform.position, player.position) == chargeRange)
            {       
                fireFeet.SetActive(false);    
                Debug.Log("Aim");   
                currentState = States.AIMING;
            }
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {       
                fireFeet.SetActive(false);       
                currentState = States.CHASING;
            }           
            if(Vector3.Distance(transform.position, player.position)> sightRange)
            {       
                fireFeet.SetActive(false);        
                Debug.Log("Where'd they go?");
                currentState = States.IDLE;
                
            }

            /*if(pos < duration)
            {
                pos += Time.deltaTime;
                transform.position = Vector3.Lerp(start, moveTarget, pos/duration);

            }
            else
            {
                agent.SetDestination(transform.position);
                agent.nextPosition = transform.position;
                agent.updatePosition = true;
                agent.updateRotation = false;
                attack.attackActive = false;
                anim.SetBool("Attacking", false);
                yield return new WaitForSeconds(1f);                
                currentState = States.FOLLOW;
            */
            
         }
            yield return new WaitForEndOfFrame();
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
        agent.updatePosition = false;
        agent.updateRotation = false;
        yield return new WaitForSeconds(4f);

        while(currentState == States.STUNNED)
        {
            
            //checking if state should change based off distance of player using the below function (IsInRange())
            if(!IsInRange(sightRange)) currentState = States.IDLE;
            else if (IsInRange(meleeRange))
            {               
                currentState = States.ATTACKING;
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

    public Quaternion RotateToPlayer()
    {
        //rotate to face player
        Vector3 dir = player.position - transform.position;
        Quaternion desiredRot = Quaternion.LookRotation(dir);

        return Quaternion.Lerp(transform.rotation, desiredRot, 0.02f);
    }
   
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
