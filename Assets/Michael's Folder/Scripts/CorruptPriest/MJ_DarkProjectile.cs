using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJ_DarkProjectile : MonoBehaviour
{
    public GameObject player;
    Rigidbody rb;

    public float launchDelay;
    public float speed;
    public float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
