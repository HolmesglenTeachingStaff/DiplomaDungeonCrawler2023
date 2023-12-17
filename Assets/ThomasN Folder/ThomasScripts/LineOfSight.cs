using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    static public bool inView = false;
    public float range = 5f;

    void Update() 
    {
        Vector3 direction = Vector3.forward;
        Ray theEyes = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

        if(Physics.Raycast(theEyes, out RaycastHit hit, range)) 
        {
            if(hit.collider.tag == "Player") 
            {
                inView = true;
            }
        }
    
    }
}
