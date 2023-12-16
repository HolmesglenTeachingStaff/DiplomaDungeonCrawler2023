using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine AI;

public class SnakeMovement : MonoBehaviour
{
    //Idle
    private int randNum;
    public float timer;

    //Hunting
    Rigidbody = rb
    public float speed;
    public GameObject player;

    //Retreat
    public float clock;
    public Transform target;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player");
        player = GameObject.FindWithTag("Player");
    }

    public void Idle()
    {
        //turn to random direction after random time
        //range from 9-351 degrees
        randNum = Random.Range(9, 351);
        IEnumerator Look()
        {
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
        if (FieldOfView.inview = true)
        {
            Vector3 direction = target.position - transform.position;

            if (direction.sqrmagnitude > 1f)
            {
                transform.Translate(direction.normalised * Time.deltaTime, Space.world)
              transform.forawrd = direction.normalised;
            }
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
        if (lookat)
        {
            transform.lookat(player.transform);
            rb.AddForce(speed * Time.deltaTime * transform.backwards);
        }
        IEnumerator Withdraw()
        {
            float elapsedTime = 0f;
            if (elapsedTime < clock)
            {
                return to idle mode
            }
        }
    }
}
