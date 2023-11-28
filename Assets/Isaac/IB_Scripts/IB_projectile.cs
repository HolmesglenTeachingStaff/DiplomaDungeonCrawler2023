using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IB_projectile : MonoBehaviour
{
    public float moveSpeed;
    public float deathTime;

    Rigidbody rb;

    public UnityEvent OnDestroy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("DestroySelf", deathTime);
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
        Destroy(gameObject, 1f);
    }
}
