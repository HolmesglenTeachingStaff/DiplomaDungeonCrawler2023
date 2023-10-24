using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RD_Projectile : MonoBehaviour
{
    public float moveSpeed;
    public float deathTime;

    Rigidbody rb;

    public UnityEvent OnDestroy;
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position += transform.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroySelf();
    }

    void DestroySelf()
    {
        OnDestroy.Invoke();
    }

}
