using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MJ_DarkProjectile : MonoBehaviour
{
    public GameObject player;
    Rigidbody rb;

    public float lifetime;   //max flight duration
    float duration;

    public float launchDelay;
    float time;             //to compare with launchDelay

    public Color colliderColor;
    public float colliderSize;

    public float speed;
    public float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        duration = 0;
    }

    private void Awake()
    {
        
    }
    private void Update()
    {
        

        while(time < launchDelay)
        {
            time += Time.deltaTime;
        }

        StartCoroutine(PROJECTILE());

        if (duration < lifetime)
        {
            
            transform.position += Vector3.forward * speed;
            duration += Time.deltaTime;
        }
        else Destroy(this.gameObject);
    }

    IEnumerator PROJECTILE()
    {

        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = colliderColor;
        Gizmos.DrawWireSphere(transform.position, colliderSize);
    }
}
