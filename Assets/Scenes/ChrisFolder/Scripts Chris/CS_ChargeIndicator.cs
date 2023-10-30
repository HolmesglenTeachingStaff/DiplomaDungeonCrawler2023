using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChargeIndicator : MonoBehaviour
{

    public Transform target;
    public float rotationSpeed;

    
   
    //add turn on/off bool

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPostition = new Vector3(target.position.x, this.transform.position.y, target.position.z);
        this.transform.LookAt(targetPostition);
    }
}
