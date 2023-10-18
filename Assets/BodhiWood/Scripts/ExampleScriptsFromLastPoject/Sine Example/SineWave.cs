using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public float speed;
    public float distance;

    Vector3 startingPoint;
    Vector3 sineMovement;

    void Start()
    {
        startingPoint = transform.position;
        speed *= Random.Range(0.5f, 1.5f);
        distance *= Random.Range(0.5f, 1.5f);
    }

    void Update()
    {
        //Move object using a sine wave
        //transform.position = Vector3.up * Mathf.Sin(Time.time);

        //Move object on the y axis with a varying speed
        //transform.position = Vector3.up * Mathf.Sin(Time.time * speed);

        //Move object on the y axis with a varying length / distance & speed
        //transform.position = distance * (Vector3.up * Mathf.Sin(Time.time * speed));


        //Move using a sine wave relative to an existing position
        sineMovement = startingPoint + distance * (Vector3.up * Mathf.Sin(Time.time * speed));

        transform.position = sineMovement;
    }
}
