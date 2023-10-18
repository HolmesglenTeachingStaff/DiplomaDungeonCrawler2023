using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFollow : MonoBehaviour
{
    //follow variables
    public float followSpeed = 5;
    public float lookSpeed = 5;
    float followDistance;
    public Transform followTarget;

    Vector3 followerPosition;

    void Update()
    {
        #region Follow target at speed, with buffer, and behind desired object
        //Determine the distance between this object, and the follow target
        followDistance = Vector3.Distance(followTarget.position, transform.position);
        
        //postion the followers behind the object being followed
        followerPosition = followTarget.position + (followTarget.forward * -1);

        if (followDistance > 1.5f)
        {
            var moveTowards = followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followerPosition, moveTowards);
        }
        #endregion

        #region Followers rotation towards followed object
        if (followDistance > 1.5f)
        {
            Vector3 targetDirection = followTarget.position - transform.position;
            float step = lookSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        #endregion
    }
}
