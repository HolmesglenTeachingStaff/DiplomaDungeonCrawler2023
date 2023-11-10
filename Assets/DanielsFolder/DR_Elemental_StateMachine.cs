using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DR_Elemental_StateMachine : MonoBehaviour
{
    #region variables
    public Transform player;
    private NavMeshAgent agent;
    private DR_ElementalPlayerTracker tracker;
    private OrbitTarget orbitTarget;
    private Animator anim;
    public float lastCharge, timeBetweenCharges, chargeTime, attackSpeed;

    public int hitCounter, maxHits;
    public float hitRecovery;
    private float lastHit;
    public DR_ElementalAttack attack;
    private Stats stats;

    public CanvasGroup healthSliders;
    #endregion

    #region States
    /// Declare states. If you add a new state to your character,
    /// remember to add a new States enum for it.
    public enum States { PUDDLE, EMERGING, FOLLOW, AIMING, ATTACKING, OVERHEATING, DIE }

    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.PUDDLE;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        tracker = GetComponentInChildren<DR_ElementalPlayerTracker>();
        orbitTarget = GetComponent<OrbitTarget>();
        orbitTarget.orbitTarget = player.gameObject;
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<Stats>();
        attack.attackActive = false;
        healthSliders.alpha = 0;
        //start the fsm
        StartCoroutine(EnemyFSM());

    }
    #endregion

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    #endregion

    #region Behaviour Coroutines
    IEnumerator PUDDLE()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour



        //UPDATE IDLE STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.PUDDLE)
        {
            if (tracker.playerEntered) currentState = States.EMERGING;
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left


    }
    IEnumerator EMERGING()
    {
        //ENTER THE IDLE STATE >
        //put any code here that you want to run at the start of the behaviour
        anim.SetBool("Emerge", true);

        yield return new WaitForSeconds(5f);

        currentState = States.FOLLOW;
        


    }
    IEnumerator FOLLOW()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        
        yield return new WaitForEndOfFrame();
        agent.updatePosition = true;
        agent.updateRotation = false;

        
        lastCharge = Time.time;
        agent.speed = 3.5f;
        agent.speed *= 0.25f;


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.FOLLOW)
        {
            if(healthSliders.alpha < 1)
            {
                healthSliders.alpha += Time.deltaTime;
            }
            if (Vector3.Distance(player.position, transform.position) <= orbitTarget.orbitDistance)
            {
                agent.speed = 3.5f * 2f;
            }
            agent.SetDestination(orbitTarget.orbitPosition);

            anim.SetFloat("Velocity", agent.velocity.magnitude);
            yield return new WaitForEndOfFrame();
            if(Time.time - lastCharge > timeBetweenCharges)
            {
                currentState = States.AIMING;
            }
            transform.rotation = RotateToPlayer();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

    }
    IEnumerator AIMING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour

        Debug.Log("I'ma Get Ya!");
        anim.SetBool("Charging", true);
        agent.SetDestination(transform.position);
        agent.updateRotation = false;
        float timer = 0;

        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.AIMING)
        {
            timer += Time.deltaTime;
            if(timer >= chargeTime)
            {
                currentState = States.ATTACKING;
                anim.SetBool("Charging", false);
                anim.SetBool("Attacking", true);
            }

            //rotate to face player
            transform.rotation = RotateToPlayer();
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    IEnumerator ATTACKING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        //pick the direction to shoot
        Vector3 moveTarget = transform.position + transform.forward * 10f;
        agent.updatePosition = false;
        attack.attackActive = true;
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 10f, LayerMask.NameToLayer("Environment")))
        {
            moveTarget = hit.point;
            moveTarget.y = transform.position.y;
        }

        float pos = 0;
        Vector3 start = transform.position;
        float duration = attackSpeed;
        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.ATTACKING)
        {
            if(pos < duration)
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
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }

    IEnumerator OVERHEATING()
    {
        //ENTER THE Chasing STATE >
        //put any code here that you want to run at the start of the behaviour
        agent.SetDestination(transform.position);
        agent.nextPosition = transform.position;
        agent.updatePosition = true;
        agent.updateRotation = true;
        attack.attackActive = false;
        anim.Play("OrbShaking");
        yield return new WaitForSeconds(1f);
        agent.speed = 1f;
        agent.SetDestination(player.position);
        float timer = 0;


        //UPDATE Chasing STATE >
        //put any code here you want to repeat during the state being active
        while (currentState == States.OVERHEATING)
        {
            if(timer <= 5)
            {
                timer += Time.deltaTime;
                agent.SetDestination(player.position);
            }
            else
            {
                attack.Explode(attack.bigAttackRange, attack.maxDamage * 2);
                currentState = States.DIE;
            }
            yield return new WaitForEndOfFrame();
        }

        //EXIT IDLE STATE >
        //write any code here you want to run when the state is left

        Debug.Log("Oh no I see the player!");
    }
    public void StartDIE()
    {
        currentState = States.DIE;
    }
    IEnumerator DIE()
    {
        //kill the object
        StatSystem.DealDamage(stats, StatSystem.DamageType.Physical, 10000, false);
        MeshRenderer[] models = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer model in models)
        {
            model.enabled = false;
        }
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    #endregion

    public void HitCount()
    {
        if (Time.time - lastHit > hitRecovery)
        {
            hitCounter++;
        }
        else hitCounter = 1;

        if(hitCounter >= maxHits)
        {
            currentState = States.OVERHEATING;
        }
    }
    public Quaternion RotateToPlayer()
    {
        //rotate to face player
        Vector3 dir = player.position - transform.position;
        Quaternion desiredRot = Quaternion.LookRotation(dir);

        return Quaternion.Lerp(transform.rotation, desiredRot, 0.02f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("AttackPlayer");
        }
    }
}
