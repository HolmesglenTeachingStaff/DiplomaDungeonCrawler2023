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

    public float chargeTime, attackSpeed;

    //public CS_DamageReactions activeDamage; //using this to turn DamageReactions on/off as it's damaging whilst in area of player
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

        //activeDamage = GetComponent<CS_DamageReactions>();

        damageBurst = GetComponent<CS_DamageReactions>();

        chargeDamage = GetComponent<CS_DamageReactions>();

        idleDone = false;
        
        currentState = States.IDLE;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        damageBurst.damageRange = meleeRange;

        //activeDamage.enabled =false;

        chargePointer.SetActive(false);
        fireFeet.SetActive(false);

        agent = GetComponent<NavMeshAgent>();
        //start the FSM (Finite State Machine)
        StartCoroutine(EnemyFSM());
        AnimCancel();
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
        AnimCancel();
        anim.SetBool("IsIdle", true);
        
        //enter the IDLE state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("Ok, no one here, lets chill");
        agent.speed = 3;
        float timer = 0;

        //add IDLE anim
        while(currentState == States.IDLE)
        {
            anim.SetBool("IsIdle", true);

            //checking if we need to chase the player
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {                
                Debug.Log("entering CHASING from IDLE");
                currentState = States.CHASING;
            }  
            if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {   
                        Debug.Log("entering AIMING from IDLE");
                        currentState = States.AIMING;
                    }
                } 
            if (timer >4) //check if we should patrol
            {              
                Debug.Log("Starting PATROL state"); 
                currentState = States.PATROLLING;
            }

            //if niether is true, we are still in IDLE and the while loop will continue, so we increase our timer and repeat
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
                 
        }

        //exit IDLE state
        //write any code here you want to run when the state is left
        Debug.Log("end IDLE state");

    }

    IEnumerator PATROLLING()
    {
        AnimCancel();
        anim.SetBool("IsWalking", true);
        //ENTER THE PATROLLING STATE >
        //put any code here that you want to run while patrolling
        
        Debug.Log("enter PATROLLING state");
        agent.speed = 3;
        agent.SetDestination(nodes[currentNode].position);

        //UPDATE PATROLLING STATE >
        //put any code here you want to repeat during patrolling state being active
        while (currentState == States.PATROLLING)
        {
            AnimCancel();
            anim.SetBool("IsWalking", true);

            //if (IsInRange(sightRange)) currentState = States.CHASING; //comparing range, making it true or false depending with function IsInRange below

             if(Vector3.Distance(transform.position, player.position) <sightRange) 
             {
                Debug.Log("entering CHASING from PATROLLING");
                currentState = States.CHASING;
             }
             if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {
                        Debug.Log("entering AIMING from PATROLLING");
                        currentState = States.AIMING;
                    }
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
        AnimCancel();
        anim.SetBool("IsRunning", true);
        //activeDamage.enabled = false;
        
        //enter the CHASING state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("began CHASING state");
        agent.speed = 3;
        agent.SetDestination(player.position);
        //lastCharge = Time.time; //this is couning up on time outside the while loop making it larger than within the while loop (TRYING YO CREATE TIMER BETWEEN CHARGES)

        //UPDATE CHASING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.CHASING)
        {

            agent.SetDestination(player.position);
           

            //checking if state should change based off distance of player using the below function (IsInRange())
            //if(!IsInRange(chargeRange)) currentState = States.IDLE;
            //if (IsInRange(meleeRange)) currentState = States.ATTACKING;
                        
            if(Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                Debug.Log("entering ATTACKING from CHASING");
                currentState = States.ATTACKING;
            }
            if(Vector3.Distance(transform.position, player.position) > meleeRange)
            {
                if(Vector3.Distance(transform.position, player.position) < sightRange)
                {
                    Debug.Log("entering CHASING from CHASING");
                    currentState = States.CHASING;
                }
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {
                        Debug.Log("entering AIMING from CHASING");
                        currentState = States.AIMING;
                    }
                }
            if(Vector3.Distance(transform.position, player.position)> chargeRange)
            {
                Debug.Log("entering IDLE from CHASING");
                currentState = States.IDLE;                
            }

            yield return new WaitForEndOfFrame();
        }

        //exit CHASING state
        //write any code here you want to run when the state is left        
        Debug.Log("end of CHASING state");

    }


    IEnumerator ATTACKING()
    {
        AnimCancel();

        Debug.Log("GOT YA!");
        //activeDamage.enabled = true;

        agent.speed = 3;
        agent.SetDestination(player.position);

        //enter the ATTACKING state
        //put any code here i want to run at the start of the behaviour
        
        
        //UPDATE ATTACKING state
        //put any code here you want to repeat during the state being active
        while(currentState == States.ATTACKING)
        {
            AnimCancel();
            anim.SetBool("IsAttacking", true);
            
            
            agent.speed = 3;
            agent.SetDestination(player.position);

            damageBurst.DamageBurst(); //accessing CS_DamageReactions script, then applying DamageBurst function

            //add ATTACK anim here
            // anim.Play("Standing_Attack");
            yield return new WaitForSeconds(2f); //the number is the time length of the animation
            

            Debug.Log("entered ATTACKING again");            

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
                //activeDamage.enabled = false;                
                Debug.Log("entering ATTACKING from ATTACKING");
                currentState = States.ATTACKING;
            }            
            if(Vector3.Distance(transform.position, player.position) > meleeRange)
            {
                if(Vector3.Distance(transform.position, player.position) < sightRange)
                {
                    Debug.Log("entering CHASING from ATTACKING");
                    //activeDamage.enabled = false;
                    currentState = States.CHASING;
                }
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {
                        Debug.Log("entering AIMING from ATTACK");
                        currentState = States.AIMING;
                    }
                }
            if(Vector3.Distance(transform.position, player.position)> chargeRange)
            {
                //activeDamage.enabled = false;
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
        AnimCancel();

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
            anim.SetBool("IsAiming", true);

            aimTimer += Time.deltaTime;
            
            if(aimTimer >= chargeTime)
            {
                chargePointer.SetActive(false);
                currentState = States.CHARGING;
            }
            
            //rotate to face player
            transform.rotation = RotateToPlayer(); //keeps rotating to player until we're over the aimTimer
            yield return new WaitForEndOfFrame();
        }

        //write any code here you want to run when the state is left

        Debug.Log("end of AIMING state");
    }


 IEnumerator CHARGING()
    {
         AnimCancel();
         
         //ENTER THE CHARGING STATE >
         //put any code here that you want to run while CHARGING
         //activeDamage.enabled = true;

         Debug.Log("beginning CHARGING state");
         //agent.speed = 8;
         //CHARGING anim here

         Vector3 moveTarget = transform.position + transform.forward * 15f;
         agent.updatePosition = false;
         //attack.attackActive = true;
         RaycastHit hit;
         
         float pos = 0; //number for going between position lerp (0 is start of pos 1 is end of pos)

         Vector3 start = transform.position; //beginning of the lerp (our agent start point)

         float duration = attackSpeed; //how fast we want to charge from 0-1 (start to finish positions, the lower the number the quicker)

         //UPDATE Chasing CHARGING
         //put any code here you want to repeat during the state being active
        while (currentState == States.CHARGING)
        {
            AnimCancel();
            anim.SetBool("IsRunningFast", true);
                        
            if(pos < duration) //checking if lerp is complete (is the path complete)
            {
                pos += Time.deltaTime; //moving through the lerp positions with time
                transform.position = Vector3.Lerp(start, moveTarget, pos/duration); //actually moving the character position with the lerp

                //charge reactions (stunned or charge damage)
                if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3f, LayerMask.NameToLayer("Environment"))) //if i get within 3 units of Environment tagged objects then do this
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    fireFeet.SetActive(false);
                    Debug.Log("entering STUNNED after CHARGING into environment");                    
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    currentState = States.STUNNED;
                }
                if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3f, LayerMask.NameToLayer("Player"))) //if i get within 3 units of Player tagged objects then do this
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    //do damage on hit?
                    chargeDamage.ChargeBurst(); //accessing CS_DamageReactions script, then applying ChargeBurst function
                    fireFeet.SetActive(false);
                    Debug.Log("entering CHASING after CHARGING hit player");                    
                    agent.updatePosition = true;
                    agent.updateRotation = true;                    
                    currentState = States.CHASING;
                }
                /*if(Vector3.Distance(transform.position, player.position) < meleeRange)
                {              
                fireFeet.SetActive(false); 
                currentState = States.ATTACKING;
                }            
                 if(Vector3.Distance(transform.position, player.position) < sightRange)
                {       
                fireFeet.SetActive(false);       
                currentState = States.CHASING;
                }
                 if(Vector3.Distance(transform.position, player.position) < chargeRange)
                {       
                fireFeet.SetActive(false);    
                Debug.Log("Aim");   
                currentState = States.AIMING;
                }
                if(Vector3.Distance(transform.position, player.position)> chargeRange)
                {       
                fireFeet.SetActive(false);        
                Debug.Log("Where'd they go?");
                currentState = States.IDLE;
                
                }*/
            }
            else
            {
                if(Vector3.Distance(transform.position, player.position) < meleeRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);                    
                    Debug.Log("entering ATTACKING from CHARGING");
                    currentState = States.ATTACKING;
                }            
                if(Vector3.Distance(transform.position, player.position) < sightRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);                    
                    Debug.Log("entering CHASING from CHARGING");
                    currentState = States.CHASING;
                }
                if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {
                        agent.SetDestination(transform.position);
                        agent.nextPosition = transform.position;
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                        fireFeet.SetActive(false);                        
                        Debug.Log("entering AIMING from CHARGING");
                        currentState = States.AIMING;
                    }
                }                
                if (Vector3.Distance(transform.position, player.position)> chargeRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);  
                    Debug.Log("entering IDLE from CHARGING");
                    currentState = States.IDLE;
                }
            }
            
            
            yield return new WaitForEndOfFrame();
        }               
            yield return new WaitForEndOfFrame();
    }
      


        
    


    IEnumerator STUNNED()
    {
        AnimCancel();
        anim.SetBool("IsHit", true);

        //activeDamage.enabled = false;
        //enter the STUNNED state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("OOUUFF");

        //UPDATE STUNNED state
        //put any code here you want to repeat during the state being active
        
        //add stunned animation here
        
        yield return new WaitForSeconds(4f);

        while(currentState == States.STUNNED)
        {
            
            //checking if state should change based off distance of player using the below function (IsInRange())
            //if(!IsInRange(chargeRange)) currentState = States.IDLE;
            /*else if (IsInRange(meleeRange))
            {              
                Debug.Log("Entering ATTACKING state"); 
                currentState = States.ATTACKING;
            }*/

            //agent.SetDestination(player.position); // i don't know why this was here??!??!?            

                if(Vector3.Distance(transform.position, player.position) < meleeRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);                    
                    Debug.Log("entering ATTACKING from STUNNED");
                    currentState = States.ATTACKING;
                }            
                if(Vector3.Distance(transform.position, player.position) < sightRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);                    
                    Debug.Log("entering CHASING from STUNNED");
                    currentState = States.CHASING;
                }
                if(Vector3.Distance(transform.position, player.position) > sightRange)
                {
                    if(Vector3.Distance(transform.position, player.position) < chargeRange)
                    {
                        agent.SetDestination(transform.position);
                        agent.nextPosition = transform.position;
                        agent.updatePosition = true;
                        agent.updateRotation = true;
                        fireFeet.SetActive(false);                        
                        Debug.Log("entering AIMING from STUNNED");
                        currentState = States.AIMING;
                    }
                }                
                if (Vector3.Distance(transform.position, player.position)> chargeRange)
                {
                    agent.SetDestination(transform.position);
                    agent.nextPosition = transform.position;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    fireFeet.SetActive(false);  
                    Debug.Log("entering IDLE from STUNNED");
                    AnimCancel();
                    currentState = States.IDLE;
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

    private void AnimCancel()
    {
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsRunningFast", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsHit", false);
        anim.SetBool("IsAiming", false);
    }
    

}
