using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JP_Projecttiles : MonoBehaviour
{
    public float moveSpeed;
    public float deathTime;
    Rigidbody rb;
    public UnityEvent OnDestroy;
   
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        Invoke("DestroySelf",deathTime);
    }

    void FixedUpdate()
    {
        rb.position+=transform.forward*moveSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        DestroySelf();
    }

    void DestroySelf()
    {
        OnDestroy.Invoke();
        Destroy(gameObject,0.1f);
    }
}
