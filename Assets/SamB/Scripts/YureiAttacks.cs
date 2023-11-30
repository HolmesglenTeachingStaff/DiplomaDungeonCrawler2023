using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YureiAttacks : MonoBehaviour
{ 
    public GameObject spellProjectile;
    public GameObject rangedProjectile;

    private float meleeConeAngle = 45f; // Adjust the angle of the melee cone
    private float meleeConeDistance = 3f; // Adjust the distance of the melee cone
    public int meleeDamage;


    private float channelTime = 2f; // Adjust the channeling time
    public float maxSpellRange = 20;

    Transform playerPosition;
    PlayerStats playerStats;

    public StatSystem.DamageType damageType;

    public ParticleSystem meleeParticles;
    public Transform projectileSpawn;


    public bool isCasting = false;



    public void Start()
    {
        //stats = GetComponent<Stats>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void MeleeAttack()
    {
        Debug.Log("Melee Attack!");
        //spell effect
        Instantiate(meleeParticles, projectileSpawn.position, projectileSpawn.rotation);

        // Detect enemies in the cone
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeConeDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {

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


    public void RangedAttack(Transform target)
    {
        GameObject projectile = Instantiate(rangedProjectile, projectileSpawn.position, projectileSpawn.rotation);         // Instantiate the projectile
        Vector3 targetPosition = target.position;         // Specify the target position when firing the projectile
        Vector3 direction = (targetPosition - transform.position).normalized;         // Calculate the direction towards the target
        projectile.GetComponent<YureiProjectiles>().Initialize(direction);         // Set the direction of the projectile

        Debug.Log("Yurei Ranged Attack!");
 
    }


    public void StartCast(Transform target)
    {
        Debug.Log("Channeling Spell...");

        isCasting = true;

        // Wait for channeling time
        StartCoroutine(StartSpellCast(target));


    }

    public IEnumerator StartSpellCast(Transform target)
    {
        isCasting = true; 
        yield return new WaitForSeconds(channelTime);

        // Check if the target is still in range
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= maxSpellRange)
        {
            isCasting = false;
            GameObject projectile = Instantiate(spellProjectile, projectileSpawn.position, projectileSpawn.rotation);         // Instantiate the projectile
            Vector3 targetPosition = target.position;         // Specify the target position when firing the projectile
            Vector3 direction = (targetPosition - transform.position).normalized;         // Calculate the direction towards the target
            projectile.GetComponent<YureiProjectiles>().Initialize(direction);         // Set the direction of the projectile

            Debug.Log("Yurei Spell Attack!");

        }

    }

}
