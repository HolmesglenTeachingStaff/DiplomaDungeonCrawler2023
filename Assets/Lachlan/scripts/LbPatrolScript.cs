using System.Xml;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
public class LbPatrolScript : MonoBehaviour


{
    private Animator anim;

    public NavMeshAgent agent;

    public Transform Player;

    public LayerMask whatIsEnvironment, whatIsPlayer;

    public enum States { PATROLLING, CHASING, ATTACKING, DEATH }

    public bool isDead = false;

  //public Stats StatScript;

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
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
     // StatScript = GetComponent<Stats>();
    }
 
    private void Update()
    {
        //Check For sight and attack range 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && !isDead) Patroling();
        if (playerInSightRange && !playerInAttackRange && !isDead) CHASING();
        if (playerInAttackRange && !isDead) AttackPlayer();
        
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
        agent.updateRotation = true;
        agent.SetDestination(Player.position);
    }
    private void AttackPlayer()
    {
        Debug.Log("No Player?");
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        if (playerInAttackRange) anim.SetTrigger("Attack");
       // else if (!playerInAttackRange && !playerInSightRange) ;// anim.SetTrigger("Attack") = false;
    }

      public void DEATH()
    {
        agent.speed = 0;
        agent.SetDestination(transform.position);
        anim.SetTrigger("DEATH");
        isDead = true;
        Debug.Log("oh no i didnt die");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}

