using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BoxInputs : MonoBehaviour
{

    //introduce variables

    //speed variable
    public float speed;
    //time variable
    public float rotationSpeed;
    //RigidBody
    public Rigidbody rb;
    //InputVector
    private Vector3 inputVector = Vector3.zero;
    

    
    void Start()
    {        
        //get rigidbody
        rb = gameObject.GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        // check for inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // combine inputs into a Vector3
        inputVector = new Vector3(h, 0, v); // inputVector is just to combine the horizontal and vertical movements to determine the direction we could move in

        inputVector.Normalize();

        //Quaternion rotation to face
        Quaternion rotation = Quaternion.LookRotation(inputVector); //this sets the belows rotation sum to where our inputs are going
        Quaternion desiredRotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed); //taking in point A as the objects own rotation, point B as the rotation we're entering, then over the time of rotation speed



        transform.rotation = desiredRotation; //after the above code is run, transforms our leader blocks rotation to the desiredRotation Lerp line above

        //transform.position += inputVector * speed * Time.deltaTime; //this code was implemented before adding curvy movement between blocks
        
    }


    // we need fixed update to use rigidbody movement
    void FixedUpdate()
    {
        // add input Vector to current position to move player

        // multiply the movement direction by speed
        rb.position += inputVector * speed * Time.deltaTime;
    }
}
