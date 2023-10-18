using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemy : BaseStats
{
    [Header("Armour")]
    public float defence = 8;
    public int armourLayers;

    float originalHealth;

    public override void Start()
    {
        base.Start();
        originalHealth = health;
    }

    public override void DoDamage(int damageAmount)
    {
        //Reduce damage based off of defence
        damageAmount -= Mathf.RoundToInt(damageAmount * (defence * 0.01f));

        //subtract the damage amount from health
        health -= damageAmount;

        //Check if dead
        if (health <= 0)
        {
            health = 0;
            Death();
        }

        base.DoDamage(damageAmount);
    }
    public override void Death()
    {
        if (armourLayers > 0)
        {
            armourLayers--;
            defence--;
            health = originalHealth;
        }
        else
        {
            Debug.Log("ME ARRRRMOUUURRRR!!!!!! T-T");
        }
    }
}
