using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS FOR EXAMPLES ONLY. COPY PORTIONS OF IT OR USE IT FOR STUDY
//DO NOT ATTACH IT TO CHARACTERS

//this class shows how to change a stat on a character based on COLLISIONS
public class HealingBlob : MonoBehaviour
{
    //store how much to heal
    public float healAmount = 10f;
    public float newMax = 200f;

    //get a reference to the GameObject
    void OnTriggerEnter(Collider other)
    {
        //check if the object has the correct tag to be a target
        if (other.tag == "Player")
        {
            //create a variable to hold the objects Stats script
            Stats statsScript = other.GetComponent<Stats>();

            //Adjust the variables in the stats script as you like.
            statsScript.currentHealth += healAmount;
            statsScript.maxHealth += statsScript.maxHealth * 0.5f;
            statsScript.maxHealth *= 1.5f;

        }
    }
    
}

//this class shows how to change a stat on a character based on the Stats already being saved as a variable
public class DR_TargetStatBoost : MonoBehaviour
{
    //create variable to hold the Stats you want to change.
    //you can assign this in the inspector
    public Stats statTarget;
    

    // Update is called once per frame
    void Update()
    {
        //increase the stat you want
        statTarget.currentArmour *= 1.01f;
        //clamp that stat you want using the Max version of it
        statTarget.currentArmour = Mathf.Clamp(statTarget.currentArmour, 0, statTarget.maxArmour);
    }
}

//This Class Shows how you can effect a group of stats using a list and loop
public class DR_HealerEnemy
{
    //Create a list of Stats to effect
    public List<Stats> goons;

    //loop through the list of stats and increase each of there stats.
    public void HealGoons()
    {
        foreach(Stats goonHealth in goons)
        {
            goonHealth.currentHealth += goonHealth.maxHealth * 0.20f;
        }
    }
}

//same as above, but with different stats
public class DR_RagerEnemy
{
    public List<Stats> goons;

    public void INCREASERAGE()
    {
        foreach (Stats goonRage in goons)
        {
            goonRage.currentBaseAttackDamage += goonRage.maxBaseAttackDamage * 0.20f;
        }
    }
    public void DIE()
    {
        foreach (Stats goonRage in goons)
        {
            goonRage.currentBaseAttackDamage = goonRage.maxBaseAttackDamage;
        }
    }
}

