using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YureiAttacks : MonoBehaviour
{ 
    public GameObject spellProjectile;
    public GameObject rangedProjectile;

    private float meleeConeAngle = 45f; // Adjust the angle of the melee cone
    private float meleeConeDistance = 3f; // Adjust the distance of the melee cone

    private float channelTime = 2f; // Adjust the channeling time
    public float maxSpellRange = 20;

    Transform playerPosition;
    Stats stats;
    Stats playerStats;

    public StatSystem.DamageType damageType;

    public int meleeDamage;
   

    public void Start()
    {
        stats = GetComponent<Stats>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    public void PerformMeleeAttack(Transform target)
    {

        // Perform melee attack logic here
        Debug.Log("Melee Attack!");

        // Perform melee attack logic here
        Debug.Log("Melee Attack!");

        // Detect enemies in the cone
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeConeDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Check if the detected object has a HealthSystem component (customize as needed)
                Stats playerStats = hitCollider.GetComponent<Stats>();

                if (playerStats != null)
                {
                    // Check if the enemy is within the cone angle
                    Vector3 directionToEnemy = hitCollider.transform.position - transform.position;
                    float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                    if (angleToEnemy <= meleeConeAngle * 0.5f)
                    {
                        // Apply damage to the enemy (customize as needed)
                        StatSystem.DealDamage(playerStats, damageType, meleeDamage);
                    }
                }
            }

        }

    }


    public void PerformRangedAttack(Transform target)
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(rangedProjectile, transform.position, Quaternion.identity);

        // Perform ranged attack logic here
        Debug.Log("Ranged Attack!");
 
    }


    public void StartCast(Transform target)
    {
        // Perform channeling spell logic here
        Debug.Log("Channeling Spell...");

        // Wait for channeling time
        StartCoroutine(StartSpellCast(target));


    }

    public IEnumerator StartSpellCast(Transform target)
    {
        yield return new WaitForSeconds(channelTime);

        // Perform spell logic after channeling
        if (target != null)
        {
            // Check if the target is still in range (you can add additional range checks here)
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= maxSpellRange)
            {
                // Instantiate a spell projectile
                GameObject newSpell = Instantiate(spellProjectile, transform.position, Quaternion.identity);

            }
        }
    }

}
