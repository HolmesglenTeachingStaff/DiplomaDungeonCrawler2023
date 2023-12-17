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


    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
         
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inradius = true;
        }
    }


    public void Idle()
    {
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
                    trigger Idle mode
                }
            } 

            
        }


        


     }
}
