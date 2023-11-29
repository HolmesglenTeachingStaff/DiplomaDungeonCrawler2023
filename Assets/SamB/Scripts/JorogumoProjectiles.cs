using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JorogumoProjectiles : MonoBehaviour
{
    public bool isRanged; //if this projectile is a ranged attack (redundant for now, but good if joro needs other projectiles)

    public float minRangedDamage;
    public float maxRangedDamage;
    
    private float rangedSpeed = 10f; // Adjust the projectile speed
    
    public StatSystem.DamageType rangedDamageType;

    private Stats playerStats;

    private Vector3 direction;
    private Vector3 initialPosition;
    public float maximumDistance = 25f; // Adjust the maximum distance for destroying the projectile
    

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

    }

    private void Start()
    {
        if (isRanged)
        {
            // Set the projectile's velocity
            GetComponent<Rigidbody>().velocity = direction * rangedSpeed;
        }

    }

    public void Initialize(Vector3 direction)
    {
        this.direction = direction;
        initialPosition = transform.position;
    }

    private void Update()
    {
     
            if (Vector3.Distance(initialPosition, transform.position) >= maximumDistance)
            {
                Destroy(gameObject);
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
