using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//take in the position of a leader and determine a desired position behind the leader with a buffer
//move the follower toward the desired position

public class CS_FollowerInputs : MonoBehaviour
{
    //introduce variables

    public Transform leader; //the object to follow
    public float speed; //how fast to follow
    public float buffer; //the distance behind the leader to follow
    Vector3 desiredPosition; //the spot behind the leader to go to
    
    public float rotationSpeed; //speed to rotate toward the block in front

    


    void Start()
    {
        
    }       
    
    void Update()
    {
        //determine the spot behind the leader
        //add the buffer distance between them
        desiredPosition = leader.position + (leader.forward * -1) * buffer; // * by -1 to make it go behind the object, * buffer to keep things uniform using the one times and buffer adding

        //move the follower
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, speed);

        //Quaternion settings, taking in the rotating positions
        Quaternion rotation = Quaternion.LookRotation(desiredPosition); // setting 'rotation' to look at the desired position (front box)
        

        //Quaternion positions, actually rotating the objects
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed); //transforming this objects rotation between itself (transform.rotation), and the previous taken in measurement of rotation above, times rotationSpeed

       
    }

}
