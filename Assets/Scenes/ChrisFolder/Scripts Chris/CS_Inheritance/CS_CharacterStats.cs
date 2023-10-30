using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CharacterStats : CS_BaseStats // on the left this script is allowed to inherit from the BaseStats script
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

    // Start is called before the first frame update
    void Start()
    {
        ChangeAttributes();
    }

    public override void DoDamage(int damageAmount)
    {
        base.DoDamage(damageAmount);
        health -= damageAmount;
        if(health <= 0)
        {
            health = 0;
            Death();
        }

    }



    void ChangeAttributes()
    {
        health = fortitude * 2 / (specialness + specialnessMod * 0.25f);
        speed = characterLevel + agility * agilityMod;
        speed = Mathf.Clamp(speed, 0, 50);
    }
   
}