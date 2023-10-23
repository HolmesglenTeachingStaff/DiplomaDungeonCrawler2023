using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SineWave : MonoBehaviour
{

    //PART 2 - speed float to change speed below
    public float speed;

    //PART 3 - length/distance float of Sin movement below
    public float distance;

    //PART 5 - Vectors so we can keep a changed position if we were to duplicate the object
    Vector3 startPosition;
    Vector3 sineMovement;

  
    
    void Start()
    {
        //PART 5 - creating objects individual start position
        startPosition = transform.position;

        //PART 6 - randomizing the speed changes of the movements
        //speed *= Random.Range(0.9f, 1.5f);
    }

    
    void Update()
    {
        //Part 1 - up and down equally for below code
        //transform.position = Vector3.up * Mathf.Sin(Time.time); 


        //PART 2 - changing the speed, needing a speed variable placed above for the code below
        //transform.position = Vector3.up * Mathf.Sin(Time.time * speed);

        //PART 3 - move on the y with a varying length/distance using added variable distance above
        //transform.position = distance * (Vector3.up * Mathf.Sin (Time.time));

        //PART 4 - move on the y with a varying length/distance and speed
        //transform.position = distance * (Vector3.up * Mathf.Sin (Time.time * speed));

        //PART 5 - move using a sine wave, relative to the objects own start position (this makes duplicates not move on the one position on game start)
        sineMovement = startPosition + distance * (Vector3.right * Mathf.Sin(Time.time * speed));

        transform.position = sineMovement;

  

    }
}
