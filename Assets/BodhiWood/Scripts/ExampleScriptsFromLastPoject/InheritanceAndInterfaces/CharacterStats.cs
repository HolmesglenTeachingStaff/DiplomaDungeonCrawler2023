using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : BaseStats
{
    [Header("Character Info")]
    public string characterName;
    public string characterInfo;
    public int characterLevel;

    [Header("Ability Modifier")]
    public int agilityMod;
    public int fortitudeMod;
    public int strengthMod;
    public int specialnessMod;
    public int intelligenceMod;
    public int attractivenessMod;
    public int faithMod;

    [Header("Attributes")]
    public float speed;
    public float power;
    public float mana;
    public float spellPower;
    public float critChance;

    void Start()
    {
        ChangeAttributes();
    }

    public override void DoDamage(int damageAmount)
    {
        base.DoDamage(damageAmount);
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Death();
            gameObject.SetActive(false);
        }
    }

    void ChangeAttributes()
    {
        health = fortitude * 2 / (specialness + specialnessMod * 0.25f);
        speed = characterLevel + agility * agilityMod;
        speed = Mathf.Clamp(speed, 0, 50);
    }
}
