using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineAI;

public class BagMovement : MonoBehaviour
{
    //Idle
    private int randNum;
    public float wait;

    //Watch
    public bool lookat = false;
    public GameObject player;

    //Flee
    public float duration;
    static public bool inradius = false;
    public float speed;
    public Transform target;

    //Search


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = GameObject.FindWithTag("Player");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inradius = true
        }
    }


    public void Idle()
    {
        //turn to random direction after random time
        //range from 0-351 degrees
        randNum = Random.Range(0, 351);
        IEnumerator Turn()
        {
            float elapsedTime = 0f;
            if (elapsedTime < duration)
            {
                transform.Rotate(0, randNum, 0);
            }

        }
    }

    public void Watch()
    {
        //if player is in line of sight watch them
        if (FieldOfView.inview = true)
        {
            lookat = true;
        }
        if (lookat)
        {
            transform.lookat(player.transform);
        }

    }

    public void Flee()
    {
        //if player is in line of sight and in radius flee
        //if player attacks flee

        if ((FieldOfView.inview) && inradius = true || Stats.TakeDamage)
        {
            Vector3 direction = transform.position - target.position;

            if (direction.sqrmagnitude > 1f)
            {
                transform.Translate(direction.normalised * Time.deltaTime, Space.world)
              transform.forawrd = direction.normalised;
            }

            StartCoroutine(Check());
        }
        IEnumerator Check()
        {
            float elapsedTime = 0f;
            if (elapsedTime < wait)
            {
                //Search
                //turn 180 degrees
                transform.Rotate(0, 180, 0);
                if (FieldOfView.inview = true)
                {

                    continue fleeing


                }

            }
        }


    }
}
