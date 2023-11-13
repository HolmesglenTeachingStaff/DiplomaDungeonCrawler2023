using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YureiProjectiles : MonoBehaviour
{
    public bool isRanged; //if this projectile is a ranged attack
    public bool isSpell; //if this projectile is a spell attack
    public Stats statBlock;

    public float minRangedDamage;
    public float maxRangedDamage;

    public float minSpellDamage;
    public float maxSpellDamage;

    private float rangedSpeed = 10f; // Adjust the projectile speed
    private float spellSpeed = 10f; // Adjust the projectile speed


    public float reduceArmour = 15;
    public float reduceSpeed = 15;
    public float reduceAttack = 15;

    public float debuffDuration = 15;

    public StatSystem.DamageType rangedDamageType; //this will just be standard damage
    public StatSystem.DamageType spellDamageType; //think this needs to be dark cause debuff


    Transform playerPosition;
    Stats playerStats;


    /* public enum DamageTargets { player, enemy, general }
    public DamageTargets damageTarget; */

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;



    }

    private void Start()
    {
        Vector3 direction = playerPosition.transform.position - transform.position;

        // Set the projectile's velocity
        GetComponent<Rigidbody>().velocity = direction * rangedSpeed;
    }


    void OnTriggerEnter(Collider other)
    {

        // Check if the projectile hits an object with a health system + get that reference
        var targetStats = other.GetComponent<Stats>();
        //if stats is empty, try getting it from the parent object or child
        if (targetStats == null) targetStats = other.GetComponentInParent<Stats>();
        if (targetStats == null) targetStats = other.GetComponentInChildren<Stats>();
        if (targetStats == null) return;
        Debug.Log("Hit");

        if (isRanged)
        {
            //play ranged particle impact/sound

            if (targetStats != null)
            {
                // Deal damage to the target
                StatSystem.DealDamage(targetStats, rangedDamageType, Random.Range(minRangedDamage, maxRangedDamage));

                // Destroy the projectile
                Destroy(gameObject);
            }

        }

        if (isSpell)
        {
            //play spell particle impact/sound

            if (targetStats != null)
            {
                // Deal damage to the target
                StatSystem.DealDamage(targetStats, rangedDamageType, Random.Range(minSpellDamage, maxSpellDamage));

                StartCoroutine(DrainSoul(statBlock, spellDamageType));

                // Destroy the projectile
                Destroy(gameObject);
            }

        }


    }

    //applies a debuff to attack, Armor (should be resist but idk how to it really) and speed. Stacks up to 3.
    public IEnumerator DrainSoul(Stats target, StatSystem.DamageType damageType)
    {
        if (target.statusEffectStacks > 3) yield return null;
        else target.statusEffectStacks++;

        //Convert reductions to percentages
        reduceAttack *= 0.01f;
        reduceArmour *= 0.01f;
        reduceSpeed *= 0.01f;


        //Determine the amount to subtract (percentage of current).
        float reduceAttackFactor = target.currentBaseAttackDamage * reduceAttack;
        float reduceArmourFactor = target.currentArmour * reduceArmour;
        float reduceSpeedFactor = target.currentSpeed * reduceSpeed;


        //reduce the stat
        target.currentBaseAttackDamage = target.currentBaseAttackDamage - reduceAttackFactor;
        target.currentSpeed = target.currentArmour - reduceArmourFactor;
        target.currentArmour = target.currentSpeed - reduceSpeedFactor;


        //wait, then reset stats/remove debuff
        yield return new WaitForSeconds(debuffDuration);
        target.currentBaseAttackDamage = target.maxBaseAttackDamage;
        target.currentSpeed = target.maxSpeed;
        target.currentArmour = target.maxArmour;

    }

    //guessing the projectile shouldnt manage the debuff, as when its destroyed it wont be able to removed the debuff? not sure

}
