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


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");   
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inradius = true;
        }
    }


    IEnumerator Idle()
    {
        //turn to random direction after random time
        //range from 0-351 degrees
        randNum = Random.Range(0, 351);       
         
        float elapsedTime = 0f;
        if (elapsedTime < duration)
        {
          transform.player(0, randNum, 0);
        }

      
    }

    public void Watch()
    {
        //if player is in line of sight watch them
        if (FieldOfView.inview = true)
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

        if ((FieldOfView.inview) && inradius || Stats.TakeDamage = true) 
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
                if (FieldOfView.inview = true)
                {
                    StartCoroutine(Run());
                    return true;
                }
                else if (FieldOfView.inview = false)
                {
                    StartCoroutine(Idle());
                    return false;
                }
            } 

            
        }


        


     }
}
