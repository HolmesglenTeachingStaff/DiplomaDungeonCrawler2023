using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadingCubeMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 0.03f;
    public float t = 0.03f;

    Vector3 inputVector = Vector3.zero;

    void Update()
    {
        //Applying inputs to variables
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //combine the inputs into a vector3
        inputVector = new Vector3(horizontal, 0, vertical);
        inputVector.Normalize();

        if (horizontal != 0 || vertical != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(inputVector);
            Quaternion desiredRotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
            transform.rotation = desiredRotation;
        }



        transform.position += inputVector * speed * Time.deltaTime;
    }
}
