using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JorogumoAttacks : MonoBehaviour
{
    public GameObject rangedProjectile;

    //private float meleeConeAngle = 45f; // Adjust the angle of the melee cone
    //private float meleeConeDistance = 3f; // Adjust the distance of the melee cone

    Transform playerPosition;
    Stats stats;
    Stats playerStats;
    SpiderlingManager spiderlingManager;
    public Transform projectileSpawn;

    public StatSystem.DamageType damageType;
    public int rangedDamage = 15;

    public float healSpellAmount = 20;
    public float maxSpellRange = 15;


    public void Start()
    {
        stats = GetComponent<Stats>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        spiderlingManager = GetComponent<SpiderlingManager>();

    }


    public void RangedAttack(Transform target)
    {
        GameObject projectile = Instantiate(rangedProjectile, projectileSpawn.position, projectileSpawn.rotation);         // Instantiate the projectile

        Vector3 targetPosition = target.position;         // Specify the target position when firing the projectile

        Vector3 direction = (targetPosition - transform.position).normalized;         // Calculate the direction towards the target

        projectile.GetComponent<JorogumoProjectiles>().Initialize(direction);         // Set the direction of the projectile


        Debug.Log("Ranged Attack!");

    }


    public void HealSpider()
    {
        // Perform channeling spell logic here
        Debug.Log("Casting heal");

        // Wait for channeling time
        StartCoroutine(StartSpellCast());

        // Choose a minion to heal (customize as needed)
        Spiderling spiderlingToHeal = FindDamagedSpiderling();

        if (spiderlingToHeal != null)
        {
            Stats spiderlingStats;
            spiderlingStats = spiderlingToHeal.GetComponent<Stats>();
            spiderlingStats.currentHealth = spiderlingStats.currentHealth + 30;
        }


    }

    public IEnumerator StartSpellCast()
    {
        //yield return new WaitForSeconds(channelTime);
        HealSpider();
        yield return null;
    }

  
    Spiderling FindDamagedSpiderling()
    {
        List<Spiderling> spiderlingsList = spiderlingManager.GetSpiderlings();

        if (spiderlingsList.Count > 0)
        {
            //Set the first spiderling in the list to be 'lowest health', then we iterate though and find the lowest health
            Spiderling lowestHealthSpiderling = spiderlingsList[0];

            foreach (var spiderling in spiderlingsList)
            {
                Stats spiderlingStats = spiderling.GetComponent<Stats>();

                if (spiderlingStats != null)
                {
                    //Compare current lowesthealthspiderlingto next in list, and update the lowestHealthspiderling if needed
                    if (spiderlingStats.currentHealth < lowestHealthSpiderling.GetComponent<Stats>().currentHealth)
                    {
                        lowestHealthSpiderling = spiderling;
                    }
                }
            }

            return lowestHealthSpiderling;
        }

        return null;
    }


}
