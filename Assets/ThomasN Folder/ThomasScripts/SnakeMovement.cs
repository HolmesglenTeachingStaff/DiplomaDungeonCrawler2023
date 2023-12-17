using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeMovement : MonoBehaviour
{
    //General
    private UnityEngine.AI.NavMeshAgent agent;

    //Idle
    private int randNum;
    public float timer;

    //Hunting
    public GameObject player;

    //Retreat
    public float clock;
    public bool lookat = false;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

     public void Idle()
     {
        //turn to random direction after random time
        //range from 9-351 degrees
        while(LineOfSight.inView = false) 
        {
            randNum = Random.Range(9, 351);

            float elapsedTime = 0f;
            if (elapsedTime < timer)
            {
                transform.Rotate(0, randNum, 0);

            }
        }

        
     }

    public void Hunting()
    {
        //Hunting
        //if player is in line of sight go towards them
        if (LineOfSight.inView = true)
        {
            agent.SetDestination(player.transform.position);
        }

    }

    public void Retreat()
    {
        //move away from player till timer runs out
        //don't look away from player
        if (LineOfSight.inView = true)
        {
            lookat = true;
        }
        if (lookat = true)
        {
            transform.player(player.transform);

            Vector3 dirtoplayer = transform.position - player.transform.position;

            Vector3 newPos = transform.position + dirtoplayer;

            agent.SetDestination(newPos);
        }
     
        float elapsedTime = 0f;
        if (elapsedTime < clock) 
        {
        
        }

        
    }
}
