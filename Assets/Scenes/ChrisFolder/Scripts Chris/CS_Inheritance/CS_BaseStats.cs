using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BaseStats : MonoBehaviour
{
    [Header("Ability Stats")]
    public int agility;
    public int fortitude;
    public int strength;
    public int specialness;
    public int intelligence;
    public int attractiveness;
    public int faith;

    [Header("Attributes")]
    public float health;

    public virtual void Start()
    {

    }

    public virtual void DoDamage(int damageAmount)
    {
        Debug.Log(gameObject.name + " was hit for " + damageAmount + " damage");
    }
    public virtual void Death()
    {
        Debug.Log(gameObject.name + " has perished");
    }
}