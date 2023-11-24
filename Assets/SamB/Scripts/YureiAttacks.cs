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
    //Stats stats;
    PlayerStats playerStats;

    public StatSystem.DamageType damageType;

    public int meleeDamage;
    public Transform projectileSpawn;

    public ParticleSystem meleeParticles;

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
        Instantiate(meleeParticles, projectileSpawn.position, Quaternion.identity);

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


    public void RangedAttack()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(rangedProjectile, projectileSpawn.position, Quaternion.identity);

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
        isCasting = true; 
        yield return new WaitForSeconds(channelTime);

        // Perform spell logic after channeling
        // Check if the target is still in range
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= maxSpellRange)
        {
            // Instantiate a spell projectile
            GameObject newSpell = Instantiate(spellProjectile, projectileSpawn.position, Quaternion.identity);

        }

    }

}
