using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ArmouredEnemy : BaseStats
{
    [Header("Armour")]
    public float defence = 8;
    public int layers;

    float originalHealth;

    // Start is called before the first frame update
    public override void  Start()
    {
        base.Start();
        originalHealth = health;
    }

    public override void DoDamage(int damageAmount)
    {
        //reduce damage by defence;
        damageAmount -= Mathf.RoundToInt(damageAmount * (defence * 0.01f));

        //subtract new damage amount from health
        health -= damageAmount;

        //check if dead
        if(health <= 0)
        {
            health = 0;
            Death();
        }
        base.DoDamage(damageAmount);
    }
    public override void Death()
    {
        if(layers > 0)
        {
            layers--;
            defence--;
            health = originalHealth;
        }
        else
        {
            Debug.Log("aaaaargh you have defeated me");
            gameObject.SetActive(false);
        }
    }
}