using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatusEffects : MonoBehaviour
{
    #region Singletone pattern
    public static StatusEffects instance;
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else { instance = this; }
    }
    #endregion
    public GameObject[] particleSystems = new GameObject[5];

    #region StatusEffects
    //DAMAGE OVER TIME EFFECT
    public IEnumerator DamageOverTime(Stats target, StatSystem.DamageType damageType, float time, float damageAmount)
    {
        if (target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(damageType), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
        int timer = 0;
        StatSystem.DealDamage(target, damageType, damageAmount, false);

        while (timer <= time)
        {
            StatSystem.DealDamage(target, damageType, damageAmount, false);
            timer++;
            yield return new WaitForSeconds(1f);
        }
        Destroy(particleInstance);
        yield return null;
    }

    //IMMOBALIZE EFFECT (can slow or completely freeze characters)
    public IEnumerator Immobalize(Stats target, StatSystem.DamageType damageType, float tempSpeed, float time)
    {
        if (target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(damageType), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
        target.currentSpeed = tempSpeed;
        yield return new WaitForSeconds(time);
        target.currentSpeed = target.maxSpeed;
        Destroy(particleInstance);
        yield return null;
    }

    //Debuff
    public IEnumerator DebuffDamage(Stats target, StatSystem.DamageType damageType, float reducePercent, float time)
    {
        if(target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(damageType), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
    
        //convert reduce amount to percentage
        reducePercent = Mathf.Clamp(reducePercent, 0f, 100f);
        reducePercent *= 0.01f;

        //Determine the amount to subtract.
        float reduceFactor = target.maxBaseAttackDamage * reducePercent;

        //reduce the buff
        target.currentBaseAttackDamage = target.maxBaseAttackDamage - reduceFactor;

        //wait to reset
        yield return new WaitForSeconds(time);
        target.currentBaseAttackDamage = target.maxBaseAttackDamage;
        Destroy(particleInstance);
    }
    //Buff

    //Heal Over Time

    //Knock back
    public IEnumerator KnockBack(Stats target, StatSystem.DamageType damageType, Vector3 damagePos, float range)
    {
        //check if the object can be pushed
        if(target.pushable == false) { yield return null; }

        NavMeshAgent agent;
        Rigidbody rb;
        rb = target.GetComponent<Rigidbody>();
        if (target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(damageType), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
        //toggle components
        //attempt to turn on Rigidbody
        if (rb != null)
        { 
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        else
        {
            yield return null;
        }
        //turn off agent if needed
        agent = target.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        Vector3 direction;

        //Determine direction and target pos
        direction = target.transform.position - damagePos;

        //pushBack
        rb.AddForce(direction * range, ForceMode.Impulse);

        yield return new WaitForSeconds(1);

        //toggle components back on
        if(rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        if(agent != null)
        {
            agent.enabled = true;
        }
        Destroy(particleInstance);
        yield return null;
        
    }

    //Wet
    public IEnumerator Wet(Stats targetStats, float time)
    {
        targetStats.isWet = true;

        yield return new WaitForSeconds(time);

        targetStats.isWet = false;
    }

    public IEnumerator EarthAttack(Stats target, float armourDamage, float time)
    {
        if (target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(StatSystem.DamageType.Earth), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
        target.currentArmour -= armourDamage;
        target.currentArmour = Mathf.Clamp(target.currentArmour, 0, target.maxArmour);

        yield return new WaitForSeconds(time);

        Destroy(particleInstance);
    }

    //LightDamage
    public IEnumerator LightAttack(Stats target, float extraDamage, float time)
    {
        if (target.statusEffectStacks >= 4) yield return null;
        else target.statusEffectStacks++;

        var particleInstance = Instantiate(PickParticleType(StatSystem.DamageType.Light), target.GetComponent<Collider>().bounds.center, target.transform.rotation, target.transform);
        target.currentHealth -= extraDamage;
        target.currentHealth = Mathf.Clamp(target.currentHealth, 0, target.maxHealth);

        yield return new WaitForSeconds(time);

        Destroy(particleInstance);
    }

    #endregion

    //VFX
    GameObject PickParticleType(StatSystem.DamageType damageType)
    {
        int indexer = (int)damageType;
        if (indexer == 0) return null;

        return particleSystems[indexer-1];
        /*if (damageType == StatSystem.DamageType.Fire)
        {
            return particleSystems[0];
        }
        if (damageType == StatSystem.DamageType.Water)
        {
            return particleSystems[1];
        }
        if (damageType == StatSystem.DamageType.Wind)
        {
            return particleSystems[2];
        }
        else
        {
            return particleSystems[3];
        }*/
    }
}

