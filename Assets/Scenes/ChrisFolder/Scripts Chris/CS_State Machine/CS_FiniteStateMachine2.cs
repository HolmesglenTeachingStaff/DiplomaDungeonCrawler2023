using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_FiniteStateMachine2 : MonoBehaviour
{

    #region variables

    public float sightRange;
    public float meleeRange;   
    public bool idleDone;
    public float[] idleTimer;

           
    public Transform player;
    public Transform shinigami;
    Vector3 startPos;
    private NavMeshAgent agent;

    public Color sightColour; //i could add my own destint colour this way. To do so, below code would be "Gizmos.color = sightColour"

    public CS_Dialogue dialogue;

     
    //patrol settings
    [SerializeField] Transform[] nodes;
    int currentNode;

    private Animator anim;

    #endregion

    #region States

    //declare states, add states where needed on the enum so it's accissible by the character
    public enum States {IDLE, FLOAT, GLOAT, WAIT, TALK}

    public States currentState;

    #endregion


    #region intialization

    //set default state
    private void Awake()
    {
        
        idleDone = false;
        
        currentState = States.IDLE;        
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        shinigami.position = startPos;
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
        agent.SetDestination(startPos);
        AnimCancel();
        anim.SetBool("IsIdle", true);
        
        //enter the IDLE state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("IDLE state entered");
        agent.speed = 3;
        float timer = 0;

        //add IDLE anim
        while(currentState == States.IDLE)
        {
            agent.SetDestination(startPos);
            anim.SetBool("IsIdle", true);

            
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {                
                Debug.Log("entering FLOAT from IDLE");
                currentState = States.FLOAT;
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {
                    
                Debug.Log("entering WAIT from IDLE");
                currentState = States.WAIT;
                   
            }          

            yield return new WaitForEndOfFrame();
                 
        }

        //exit IDLE state      
        Debug.Log("end IDLE state");

    }

    IEnumerator WAIT()
    {
        AnimCancel();
        anim.SetBool("IsWaiting", true);
        
        
        Debug.Log("enter WAIT state");
        agent.speed = 3;
        agent.SetDestination(shinigami.position);
        float timer = 0;

        //UPDATE WAIT STATE >
        //put any code here you want to repeat during WAIT state being active
        while (currentState == States.WAIT)
        {
            agent.SetDestination(shinigami.position);
            AnimCancel();
            anim.SetBool("IsWaiting", true);
           
            if(Vector3.Distance(transform.position, player.position) <sightRange) 
            {
                Debug.Log("entering FLOAT from WAIT");
                currentState = States.FLOAT;
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange && timer >4)
            {
                    
                Debug.Log("entering IDLE from WAIT");
                currentState = States.IDLE;

            }
            else if(Vector3.Distance(transform.position, player.position) > sightRange)
            {
                    
                Debug.Log("entering WAIT from WAIT");
                currentState = States.WAIT;

            }

            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("end WAIT state");
    }

   
    IEnumerator FLOAT()
    {  
        AnimCancel();
        anim.SetBool("IsFloating", true);
  
        //enter the FLOAT state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("began CHASING state");
        agent.speed = 3;
        agent.SetDestination(player.position);
       
        //UPDATE FLOAT state
        //put any code here you want to repeat during the state being active
        while(currentState == States.FLOAT)
        { 
            agent.SetDestination(player.position);
                        
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                Debug.Log("entering FLOAT from FLOAT");
                currentState = States.FLOAT;
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {

                Debug.Log("entering WAIT from FLOAT");
                currentState = States.WAIT;

            }

            yield return new WaitForEndOfFrame();
        }

        //exit FLOAT state
        //write any code here you want to run when the state is left
        Debug.Log("end of FLOAT state");
    }

    IEnumerator TALK()
    {  
        AnimCancel();
        anim.SetBool("IsTalking", true);
  
        //enter the TALK state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("began TALK state");
        agent.speed = 3;
        agent.SetDestination(player.position);
       
        //UPDATE TALK state
        //put any code here you want to repeat during the state being active
        while(currentState == States.FLOAT)
        { 
            agent.SetDestination(player.position);
                        
            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                Debug.Log("entering FLOAT from TALK");
                currentState = States.FLOAT;
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {

                Debug.Log("entering WAIT from TALK");
                currentState = States.WAIT;

            }

            yield return new WaitForEndOfFrame();
        }

        //exit TALK state
        //write any code here you want to run when the state is left
        Debug.Log("end of TALK state");
    }


    IEnumerator GLOAT()
    {

        AnimCancel();

        Debug.Log("GOT YA!");

        agent.speed = 3;
        agent.SetDestination(player.position);

        //enter the GLOAT state
        //put any code here i want to run at the start of the behaviour
        
        
        //UPDATE GLOAT state
        //put any code here you want to repeat during the state being active
        while(currentState == States.GLOAT)
        {
            AnimCancel();
            anim.SetBool("IsGloat", true);
            
            
            agent.speed = 3;
            agent.SetDestination(player.position);
 
            yield return new WaitForSeconds(2f); //the number is the time length of the animation
            

            Debug.Log("entered GLOAT again");            


            if(Vector3.Distance(transform.position, player.position) < sightRange)
            {
                Debug.Log("entering FLOAT from GLOAT");
                currentState = States.FLOAT;
            }

            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {                    
                Debug.Log("entering WAIT from GLOAT");
                currentState = States.WAIT; 
            }
 
            
            yield return new WaitForEndOfFrame();
        }

        //exit ATTACKING state
        //write any code here you want to run when the state is left
        Debug.Log("restarting GLOAT state");

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
        anim.SetBool("IsWaiting", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsFloating", false);
        anim.SetBool("IsGloat", false);        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("AttackPlayer with collider");
        }
    }

}
