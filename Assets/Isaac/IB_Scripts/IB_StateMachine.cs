using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IB_StateMachine : MonoBehaviour
{
    #region variables
    public float sightRange;
    public float meleeRange;
    public float buffer;
    public Stats statScript;
    public Transform player;
    public NavMeshAgent agent;
    public Color sightColor;

    public IB_DamageReactions KnockBack;
    public GameObject tenguM;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public GameObject Weapon;
    public Material mat;
    public Color newColor;

    private Animator anim;
    #endregion

    #region States
    //Declare states. If you add a new sate to your character, remember to add a new States enum for it.
    public enum States { IDLE, CHASING, ATTACKING, SPAWNING, DEATH }
    public States currentState;
    #endregion

    #region Initialization
    //set default state
    private void Awake()
    {
        currentState = States.IDLE;
    }
    #endregion

    void Start()
    {
        statScript = GetComponent<Stats>();
        KnockBack = GetComponent<IB_DamageReactions>();
        agent = GetComponent<NavMeshAgent>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponentInChildren<Animator>();
        Weapon.GetComponent<BoxCollider>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //start of fsm
        StartCoroutine(EnemyFSM());
    }

    #region Finite StateMachine
    IEnumerator EnemyFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }

    }
    #endregion

    #region Behavior Coroutines
    IEnumerator IDLE()
    {
        while (currentState == States.IDLE)
        {
            anim.Play("Idle");
            //Check player distance
            if (Vector3.Distance(transform.position, player.position) < sightRange)
            {
                //Change state
                currentState = States.CHASING;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator CHASING()
    {
        while (currentState == States.CHASING)
        {
            agent.SetDestination(player.position + player.transform.right * buffer);
            anim.Play("Walking");

            if (Vector3.Distance(transform.position, player.position) > sightRange)
            {
                currentState = States.IDLE;
            }

            if (Vector3.Distance(transform.position, player.position) < meleeRange)
            {
                currentState = States.ATTACKING;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator ATTACKING()
    {
        anim.Play("Smash");
        Weapon.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(2f);
        Weapon.GetComponent<BoxCollider>().enabled = false;
        currentState = States.CHASING;
    }
    IEnumerator SPAWNING()
    {
        while (currentState == States.SPAWNING)
        {
            //Animation, Knockback and emission
            yield return new WaitForSeconds(0.01f);
            anim.Play("Casting");
            KnockBack.DamageBurst();
            mat.SetFloat("Float", 1);
            yield return new WaitForSeconds(2.5f);

            //spawning Minions
            Instantiate(tenguM, spawnPoint1.position, spawnPoint1.rotation);
            Instantiate(tenguM, spawnPoint2.position, spawnPoint2.rotation);
            Instantiate(tenguM, spawnPoint3.position, spawnPoint3.rotation);

            yield return new WaitForEndOfFrame();
            mat.SetFloat("Float", 0);
            currentState = States.CHASING;
        }
    }
    IEnumerator DEATH()
    {
        while (currentState == States.DEATH)
        {
            anim.Play("Death");
            yield return new WaitForSeconds(10f);
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    public void Death()
    {
        StopAllCoroutines();
        currentState = States.DEATH;
        StartCoroutine(DEATH());
    }

    public void CurrentHealth()
    {
        if (statScript.currentHealth <= statScript.maxHealth * 0.5)
        {
            currentState = States.SPAWNING;
        }
    }
}
