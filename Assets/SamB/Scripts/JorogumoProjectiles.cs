using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JorogumoProjectiles : MonoBehaviour
{
    public bool isRanged; //if this projectile is a ranged attack (redundant for now, but good if joro needs other projectiles)

    public float minRangedDamage;
    public float maxRangedDamage;
    
    private float rangedSpeed = 10f; // Adjust the projectile speed
    
    public StatSystem.DamageType rangedDamageType; //this will just be standard damage

    private Transform playerPosition;
    private Stats playerStats;

    /* public enum DamageTargets { player, enemy, general }
    public DamageTargets damageTarget; */

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Start()
    {
        // Calculate the direction towards the target
        Vector3 direction = (playerPosition.transform.position - transform.position).normalized;

        if (isRanged)
        {
            // Set the projectile's velocity
            GetComponent<Rigidbody>().velocity = direction * rangedSpeed;
        }
        
    }


    void OnTriggerEnter(Collider other)
    {

        // Check if the projectile hits an object with a health system + get that reference
        var targetStats = other.GetComponent<PlayerStats>();

        //if stats is empty, try getting it from the parent object or child
        if (targetStats == null) targetStats = other.GetComponentInParent<PlayerStats>();
        if (targetStats == null) targetStats = other.GetComponentInChildren<PlayerStats>();
        if (targetStats == null) return;
        Debug.Log("Hit");


        if (isRanged)
        {
            //play ranged particle impact/sound

            if (targetStats != null)
            {
                // Deal damage to the target
                StatSystem.DealDamage(targetStats, rangedDamageType, Random.Range(minRangedDamage, maxRangedDamage));


            }

        }
        
        Destroy(gameObject);

    }

}
