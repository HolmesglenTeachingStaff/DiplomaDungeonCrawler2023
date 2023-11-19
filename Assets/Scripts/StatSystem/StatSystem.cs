using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatSystem
{
    #region Damges and Risistances
    public enum DamageType
    {
        Physical,
        Fire, //DOTs
        Water, //Wet effect and slowing
        Wind,//KnockBack + Freeze if Wet (freeze immobalizes for a time)
        Earth, //Extra Damage to Armour
        Light, //Extra Damage against Dark
        Dark, // debuffs and slows
        Ice
    }
    public enum Resistance
    {
        Resistant,
        Neutral,
        Vulnerable,
        Immune
    }
    #endregion

    /// <summary>
    /// Call this function to damage an entity in the game. This function will process resistances of the entity against your damage type and out put a damage amount accordingly.
    /// The function will automatically apply status effects where relivant unless you specify not to.
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="damageType"></param>
    /// <param name="amount"></param>
    /// <param name="Effect"></param>
    public static void DealDamage(Stats targetStats, DamageType damageType, float amount, bool Effect = true, Vector3 damagePos = default)
    {
        if (targetStats.immune == true) return;
        //cache variables
        float realDamage = 0;
        Resistance targetResistance;
        targetStats.resistances.TryGetValue(damageType.ToString(), out targetResistance);

        //Process resistance and damage
        if (targetResistance == Resistance.Neutral)
        {
            if (Effect == true)
            {
                if (damageType == DamageType.Fire)
                {
                    BeginDamageOverTime(targetStats, damageType, 5, 2);
                }
                if (damageType == DamageType.Water)
                {
                    WaterDamage(targetStats, 5);
                }
                if (damageType == DamageType.Wind)
                {
                    WindDamage(targetStats, damagePos, 10, 3);
                }
                if(damageType == DamageType.Earth)
                {
                    EarthDamage(targetStats, 15, 2);
                }
                
            }

            realDamage = amount;
        }
        else if (targetResistance == Resistance.Vulnerable)
        {
            if (Effect == true)
            {
                if (damageType == DamageType.Fire)
                {
                    BeginDamageOverTime(targetStats, damageType, 5, 2);
                }
                if (damageType == DamageType.Water)
                {
                    WaterDamage(targetStats, 5);
                }
                if(damageType == DamageType.Wind)
                {
                    WindDamage(targetStats, damagePos, 10, 3);
                }
                if (damageType == DamageType.Earth)
                {
                    EarthDamage(targetStats, 15, 2);
                }
                if(damageType == DamageType.Light)
                {
                    LightDamage(targetStats, damagePos, 10, 10, 1);
                }
                if (damageType == DamageType.Dark)
                {
                    BeginDarkDebuff(targetStats, 0.5f, 50, 5);
                }
            }

            realDamage = amount * 1.5f;
        }
        else if (targetResistance == Resistance.Resistant) realDamage = amount * 0.5f;
        else if (targetResistance == Resistance.Immune) return;
        //add obsorbs resistance type

        //process armour
        if (targetStats.currentArmour > 0)
        {
            targetStats.currentArmour -= realDamage * 0.8f;
            targetStats.TakeDamage(realDamage * 0.2f);
        }
        else
        {
            targetStats.TakeDamage(realDamage);
        }

    }

    #region Status Effect Triggers
    /// <summary>
    /// Deal a damage over time effect on a target. This is automatically called by deal damage. Only call this directly if you need to customise the damage amount and time of the effect
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="damageType"></param>
    /// <param name="time"></param>
    /// <param name="damageAmount"></param>
    public static void BeginDamageOverTime(Stats targetStats, DamageType damageType, float time, float damageAmount)
    {

        if (notResistant(targetStats, damageType))
        {
            if (StatusEffects.instance != null)
                StatusEffects.instance.StartCoroutine(StatusEffects.instance.DamageOverTime(targetStats, damageType, time, damageAmount));
        }

    }
    /// <summary>
    /// Temporarily slows a target down. Automatically called by DealDamage. Only call this directly if you need to customise the speed and time, or deal the effect with no damage
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="damageType"></param>
    /// <param name="tempSpeed"></param>
    /// <param name="time"></param>
    public static void BeginImobalize(Stats targetStats, DamageType damageType, float tempSpeed, float time)
    {
        if (notResistant(targetStats, damageType))
        {
            if (StatusEffects.instance != null)
                StatusEffects.instance.StartCoroutine(StatusEffects.instance.Immobalize(targetStats, damageType, tempSpeed, time));
        }
    }
    /// <summary>
    /// Apply Dark Damage debuff. Slow the enemy down and also debuff its health. Automatically called by DealDamage if applicable according to stats.
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="damageType"></param>
    /// <returns></returns>
    public static void BeginDarkDebuff(Stats targetStats, float tempSpeed, float reductionPercent, float time)
    {
        if(notResistant(targetStats, DamageType.Dark))
        {
            if(StatusEffects.instance != null)
            {
                StatusEffects.instance.StartCoroutine(StatusEffects.instance.Immobalize(targetStats, DamageType.Dark, tempSpeed, time));
                StatusEffects.instance.StartCoroutine(StatusEffects.instance.DebuffDamage(targetStats, DamageType.Dark, reductionPercent, time));
            }
        }
    }
    /// <summary>
    /// Begin the water damage effect. If the target is not wet, wet them and slow them down. If they are wet, freeze them
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="time"></param>
    public static void WaterDamage(Stats targetStats, float time)
    {
        if (notResistant(targetStats, DamageType.Water))
        {
            if (StatusEffects.instance != null)
            {
                StatusEffects statEffect = StatusEffects.instance;

                //check if wet
                if(targetStats.isWet)
                {
                    return;
                }
                else
                {
                    statEffect.StartCoroutine(statEffect.Immobalize(targetStats, DamageType.Water, 0.5f, time));
                    statEffect.StartCoroutine(statEffect.Wet(targetStats,time));
                }
            }
        }
    }
    public static void WindDamage(Stats targetStats, Vector3 damagePos, float force, float time)
    {
        if (notResistant(targetStats, DamageType.Wind))
        {
            if (StatusEffects.instance != null)
            {
                StatusEffects statEffect = StatusEffects.instance;

                //check if wet
                if (targetStats.isWet)
                {
                    //freeze
                    statEffect.StartCoroutine(statEffect.Immobalize(targetStats, DamageType.Ice, 0, time));
                }
                else
                {
                    statEffect.StartCoroutine(statEffect.KnockBack(targetStats, DamageType.Wind, damagePos, force));
                    //statEffect.StartCoroutine(statEffect.Wet(targetStats, time));
                }
            }
        }
    }

    public static void LightDamage(Stats targetStats, Vector3 damagePos, float force, float time, float extraDamage)
    {
        if (notResistant(targetStats, DamageType.Light))
        {
            if (StatusEffects.instance != null)
            {
                StatusEffects statEffect = StatusEffects.instance;
                statEffect.StartCoroutine(statEffect.KnockBack(targetStats, DamageType.Light, damagePos, force));
                statEffect.StartCoroutine(statEffect.LightAttack(targetStats, extraDamage, time));
            }
        }
    }

    public static void EarthDamage(Stats targetStats, float armourDamage, float time)
    {
        if (notResistant(targetStats, DamageType.Earth))
        {
            if (StatusEffects.instance != null)
            {
                StatusEffects statEffect = StatusEffects.instance;
                statEffect.StartCoroutine(statEffect.EarthAttack(targetStats, armourDamage, time));
            }
        }
    }

    public static bool notResistant(Stats targetStats, DamageType damageType)
    {
        Resistance targetResistance;
        targetStats.resistances.TryGetValue(damageType.ToString(), out targetResistance);

        if (targetResistance == Resistance.Neutral || targetResistance == Resistance.Vulnerable)
            return true;
        else
            return false;
    }
    #endregion
    /// <summary>
    /// Used to replenish health, armour, or speed. Enter the name of the stat you want to increase, and the amount you would like to increase it to. This is capped at whatever you have set the max value to.
    /// </summary>
    /// <param name="targetStats"></param>
    /// <param name="statToBuff"></param>
    /// <param name="amountToBuff"></param>
    public static void BuffStat(Stats targetStats, string statToBuff, float amountToBuff)
    {
        //convert the string to all lowercase
        string statName = statToBuff.ToLower();
        //check wich stat to buff
        if (statName == "health")
        {

            targetStats.currentHealth = Mathf.Clamp(targetStats.currentHealth + amountToBuff, 0f, targetStats.maxHealth);
        }
        else if (statName == "speed")
        {
            targetStats.currentSpeed = Mathf.Clamp(targetStats.currentSpeed + amountToBuff, 0f, targetStats.maxSpeed);
        }
        else if (statName == "armour")
        {
            targetStats.currentArmour = Mathf.Clamp(targetStats.currentArmour + amountToBuff, 0f, targetStats.maxArmour);
        }
        targetStats.OnBuffRecieved.Invoke();

    }
}
