using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Events;

public class CS_FiniteStateMachine2 : MonoBehaviour
{

    #region variables

    public float sightRange;
    public float talkRange;
    public bool idleDone;
    public float[] idleTimer;

    public float meleeRange;
    public float damageRange;

    public UnityEvent OnFloating;

           
    public Transform player;
    public Transform shinigami;
    Vector3 startPos;
    private NavMeshAgent agent;

    public GameObject gloatVictoryScreen;

    public CS_DamageReactions damageBurst;
    public CS_DamageReactions attack;

    public CS_LookAtPos looking;

    public Color sightColour; //i could add my own destint colour this way. To do so, below code would be "Gizmos.color = sightColour"

    //public CS_Dialogue dialogue;

     
    //patrol settings
    [SerializeField] Transform[] nodes;
    int currentNode;

    private Animator anim;

    //TYPEWRITER VARIABLES BELOW
    
    //get a refference to the text UI
    public TMP_Text textUI;

    //add dialogue canvas here
    public GameObject dialogueCanvas;

    //hold a collection of messages
    public string[] messages;

    //customize timing, these below will be used to customize wait times between lettering or messages appearing
    public float letterDelay;
    public float messageDelay;    

 
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

        damageBurst = GetComponent<CS_DamageReactions>();

        idleDone = false;
        
        currentState = States.IDLE;        
    }

    private void Start()
    {
        damageBurst.damageRange = meleeRange;
        anim = GetComponentInChildren<Animator>();
        shinigami.position = startPos;
        agent = GetComponent<NavMeshAgent>();
        attack.attackActive = false;
        gloatVictoryScreen.SetActive(false);

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
        currentNode = 0;
        agent.updatePosition = false;
        attack.attackActive = false;
        dialogueCanvas.SetActive(false);
        gloatVictoryScreen.SetActive(false);
        transform.position = startPos;
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
            gloatVictoryScreen.SetActive(false);
            attack.attackActive = false;
            dialogueCanvas.SetActive(false);
            transform.position = startPos;
            anim.SetBool("IsIdle", true);


            agent.SetDestination(transform.position);
            agent.nextPosition = transform.position;

            agent.updatePosition = true;

            
            if(Vector3.Distance(transform.position, player.position) < talkRange)
            {                
                Debug.Log("entering TALK from IDLE");
                currentState = States.TALK;
            }
            if(Vector3.Distance(transform.position, player.position) > talkRange)
            {
                if(Vector3.Distance(transform.position, player.position) < sightRange)
                {                    
                    Debug.Log("entering IDLE from IDLE");
                    currentState = States.IDLE;                   
                }
            }
            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {                    
                Debug.Log("entering IDLE from IDLE");
                currentState = States.IDLE;                   
            }

            yield return new WaitForEndOfFrame();
                 
        }

        //exit IDLE state      
        Debug.Log("end IDLE state");

    }

    IEnumerator WAIT()
    {
        attack.attackActive = false;
        dialogueCanvas.SetActive(false);
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
            attack.attackActive = false;
            dialogueCanvas.SetActive(false);
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
        agent.updatePosition = true;
        attack.attackActive = false;
        dialogueCanvas.SetActive(false);
        AnimCancel();
        anim.SetBool("IsFloating", true);
  
        //enter the FLOAT state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("began FLOAT state");
        agent.speed = 3;
        agent.SetDestination(nodes[currentNode].position);
        

        //UPDATE FLOAT STATE >
        //put any code here you want to repeat during FLOAT state being active
        while (currentState == States.FLOAT)
        {
            attack.attackActive = false;
            AnimCancel();
            anim.SetBool("IsFloating", true);
            dialogueCanvas.SetActive(false);
            //agent.SetDestination(nodes[currentNode].position);
                      
            if(Vector3.Distance(transform.position, player.position) > sightRange)
            {
                Debug.Log("entering WAIT from FLOAT");
                currentState = States.WAIT;
            }
            if(Vector3.Distance(transform.position, player.position) <sightRange) 
            {
                if (currentNode == nodes.Length -1)
                {
                    Debug.Log("entering GLOAT from FLOAT");
                    currentState = States.GLOAT;
                }
                /*for (int currentNode = 0; currentNode < nodes.Length; currentNode ++)
                {
                    Debug.Log("entering FLOAT from FLOAT");
                    currentState = States.FLOAT;
                }*/
                
                
            }
            if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//pathPending is when the agent is still in the process of picking a path
                {
                    yield return new WaitForSeconds(1); //wait at node, then move on
                    currentNode = (currentNode + 1) % nodes.Length; //this divides the first number by the second number, starting from node 0 going upward, resseting the nodes to 0 when they're completed through.

                    //currentNode++; //increasing node count
                    agent.SetDestination(nodes[currentNode].position);
                }
            
            /*if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//pathPending is when the agent is still in the process of picking a path
            {
                yield return new WaitForSeconds(Random.Range(0, 5)); //randomizes the time character waits between nodes
                agent.speed = Random.Range(1, 4); //randomizes the speed the character moves between nodes

                //currentNode = Random.Range(0, nodes.Length); //this would randomize the nodes the npc goes between

                currentNode = (currentNode + 1) % nodes.Length; //this divides the first number by the second number, starting from node 0 going upward, resseting the nodes to 0 when they're completed through.

                //currentNode++; //increasing node count
                agent.SetDestination(nodes[currentNode].position);
            }*/

            yield return new WaitForEndOfFrame();
        }

        //exit FLOAT state
        //write any code here you want to run when the state is left
        Debug.Log("end of FLOAT state");
    }

    IEnumerator TALK()  
    {
        attack.attackActive = false;
        dialogueCanvas.SetActive(true);
        AnimCancel();
        anim.SetBool("IsTalking", true);

        //run the typewriter coroutine
        StartCoroutine(TipeWriter());
  
        //enter the TALK state
        //put any code here i want to run at the start of the behaviour
        Debug.Log("began TALK state");
        agent.speed = 3;
        transform.position = startPos;

        //this amount is how long the text should read for
        yield return new WaitForSeconds(30);
       
        //UPDATE TALK state
        //put any code here you want to repeat during the state being active
        while(currentState == States.TALK)
        {
            attack.attackActive = false;
            //dialogueCanvas.SetActive(true);
            transform.position = startPos;
                        
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
        dialogueCanvas.SetActive(false);
        AnimCancel();
        anim.SetBool("IsGloat", true);

        Debug.Log("GOT YA!");

        agent.speed = 3;
        agent.SetDestination(player.position);
        attack.attackActive = true;
        damageBurst.DamageBurst();
    

        yield return new WaitForSeconds(2f);

        gloatVictoryScreen.SetActive(true);

        yield return new WaitForSeconds(2f);

        

        //enter the GLOAT state
                
        //UPDATE GLOAT state
        //put any code here you want to repeat during the state being active
        while(currentState == States.GLOAT)
        {
            attack.attackActive = true;
            dialogueCanvas.SetActive(false);
            AnimCancel();
            anim.SetBool("IsGloat", true);            
            
            agent.speed = 3;
            agent.SetDestination(player.position);
            gloatVictoryScreen.SetActive(true);
 
            yield return new WaitForSeconds(2f); //the number is the time length of the animation
           
            
            Debug.Log("entering IDLE from GLOAT");
            currentState = States.IDLE;
 
            
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
        Gizmos.DrawWireSphere(transform.position, sightRange); //Gizmos is a class itself, this line of code allows us to see the sightRange or talkRange above in the variables

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, talkRange);
        
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
        anim.SetBool("IsTalking", false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("AttackPlayer with collider");
        }
    }

    IEnumerator TipeWriter()
    {
        //select the first message
        textUI.text = ""; //this makes the initial message nothing as a place holder

        //create a loop that runs through every message
        for(int i = 0; i < messages.Length; i ++)
        {     

        //in the loop create an array of every character in the message
        char[] chars = messages[i].ToCharArray();

        //loop through each character in the character array
        for(int j = 0; j < chars.Length; j++)
        {        
        //for each character add it to a string, pause, then update UI with a new string
        textUI.text += chars[j].ToString();
        yield return new WaitForSeconds(letterDelay);

        //when all characters are displayed on the screen end the loop
        }
        
        //when the character loop is ended, pause, then wipe the message, then move to the next message and repeat the loop
        yield return new WaitForSeconds(messageDelay); //the timer chosen above in "messageDelay"
        textUI.text = ""; //this is to give nothing more, in order to wait for the next message
        }

        //when all messages are complete, end the corouting

        yield return null;
    }
    


}
