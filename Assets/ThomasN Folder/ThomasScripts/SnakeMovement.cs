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
        player = GameObject.FindWithTag("Player");
    }

     IEnumerator Idle()
     {
        //turn to random direction after random time
        //range from 9-351 degrees
        randNum = Random.Range(9, 351);       
        
        float elapsedTime = 0f;
        if (elapsedTime < timer)
        {
          transform.Rotate(0, randNum, 0);
            return;
        }
        
     }

    public void Hunting()
    {
        //Hunting
        //if player is in line of sight go towards them
        if (FieldOfView.inview = true)
        {
            agent.SetDestination(player.transform.position);
        }

    }

    public void Retreat()
    {
        //move away from player till timer runs out
        //don't look away from player
        if (FieldOfView.inview = true)
        {
            lookat = true;
        }
        if (lookat = true)
        {
            transform.player(player.transform);

            StartCoroutine(Withdraw());
        }
        IEnumerator Withdraw()
        {
            float elapsedTime = 0f;
            if (elapsedTime < clock)
            {
                StartCoroutine(Idle());
                return true;
            }
        }
    }
}
