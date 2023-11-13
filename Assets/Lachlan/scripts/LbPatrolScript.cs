using System.Xml;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
public class LbPatrolScript : MonoBehaviour


{
    public NavMeshAgent agent;

    public Transform Player;

    public LayerMask whatIsEnvironment, whatIsPlayer;

    public enum States { PATROLLING, CHASING, ATTACKING, DEATH }

    public States currentState;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //attacking
    public float timeBetweenAttack;
    bool alreadyAttacked;

    //States

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Start()

    {
        Player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

   
    private void Update()
    {
        //Check For sight and attack range 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) CHASING();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpointreached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsEnvironment))
            walkPointSet = true;
    }
   private void CHASING()
    {
        //ENTER THE CHASING STATE >
        //put any code here that you want to run at the start of the behaviour
        agent.updateRotation = true;
        agent.SetDestination(Player.position);


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        //while (currentState == States.CHASING) ;
        //{
           // if ((attackRange) && Time.time - lastAttack > timeBetweenAttacks) currentState = States.ATTACKING;
           // else if (!IsInRange(sightRange)) currentState = States.PATROLLING;
            //agent.SetDestination(Player.position);
          //  yield return new WaitForEndOfFrame();
        //}

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    private void AttackPlayer()
    {

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
}

