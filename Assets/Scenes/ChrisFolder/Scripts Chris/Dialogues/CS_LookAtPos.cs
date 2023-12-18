using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LookAtPos : MonoBehaviour
{
    public Color sightColor;
    public float sightRange;
    public Transform player;
    public float lookAtSpeed;
    
    private Coroutine LookCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (IsInRange(sightRange))
        {
            Quaternion rotTarget = Quaternion.LookRotation(player.position - this.transform.position);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, lookAtSpeed * Time.deltaTime);

            //Vector3 direction = player.position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lookAtSpeed * Time.deltaTime);
        }
    }
    
    bool IsInRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = sightColor;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
