using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MJ_DarkProjectile : MonoBehaviour
{
    public GameObject player;
    Rigidbody rb;

    public float lifetime; //max flight duration
    float duration;
    public float launchDelay;

    public float speed;
    public float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        duration = 0;
    }

    private void Update()
    {
        if (duration < lifetime)
        {
            //move towards target
            duration += Time.deltaTime;
        }
        else Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //deal damage on player
            //StatSystem.DealDamage(targetStats, StatSystem.DamageType.Dark, damage);
        }
        else Destroy(this.gameObject);
    }
}
