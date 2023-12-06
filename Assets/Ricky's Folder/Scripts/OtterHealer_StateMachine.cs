using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtterHealer_StateMachine : MonoBehaviour
{

    public float sightRange;
    public float healingRange;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
