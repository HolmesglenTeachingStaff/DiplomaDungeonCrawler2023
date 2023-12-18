using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BagMovement : MonoBehaviour
{
    //General
    private UnityEngine.AI.NavMeshAgent agent;

    //Idle
    private int randNum;
    public float wait;

    //Watch
    public bool lookat = false;
    public GameObject player;

    //Flee
    public float duration;
    static public bool inradius = false;

    //Search


    public BagYokaiState DefaultState;
    private BagYokaiState _state;

    public BagYokaiState State 
    {
        get 
        {
            return _state;
        }
        set 
        {
            OnStateChange?.Invoke(_state, value);
            _state = value;
        }
    }

    public delegate void StateChangeEvent(BagYokaiState oldstate, BagYokaiState newState);
    public StateChangeEvent OnStateChange;


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        OnStateChange += HandleStateChange;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inradius = true;
        }
    }

    private void OnDisable() 
    {
        _state = DefaultState;
    }

    public void Idle()
    {
        OnStateChange?.Invoke(BagYokaiState.Idle, DefaultState);
        //turn to random direction after random time
        //range from 0-351 degrees
        while(LineOfSight.inView = false) 
        {
            randNum = Random.Range(0, 351);

            float elapsedTime = 0f;
            if (elapsedTime < duration)
            {
                transform.player(0, randNum, 0);
            }
        }

    }

    public void Watch()
    {
        //if player is in line of sight watch them
        if (LineOfSight.inView = true)
        {
            lookat = true;
        }
        if (lookat = true)
        {
            transform.player(player.transform);
        }
        else if (lookat = false)
        {
            Idle();
        }
    }

     public void Flee()
     {
        //if player is in line of sight and in radius flee
        //if player attacks flee

        if (LineOfSight.inView && inradius || Stats.TakeDamage) 
        {
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            Vector3 dirtoplayer = transform.position - player.transform.position;

            Vector3 newPos = transform.position + dirtoplayer;

            agent.SetDestination(newPos);

            StartCoroutine(Search());

        }  
        IEnumerator Search() 
        {
          float elapsedTime = 0f;
          if (elapsedTime < wait)
          {
            //Search
            //turn 180 degrees
            transform.Rotate(0, 180, 0);
            if (LineOfSight.inView = true)
            {
              StartCoroutine(Run());

            }
            else if (LineOfSight.inView = false)
            {
              Idle();
            }
          }


        }

        private void HandleStateChange(EnemyState oldState, EnemyState newState) 
        {
          if(oldState != newState)
        }
        


     }
}
