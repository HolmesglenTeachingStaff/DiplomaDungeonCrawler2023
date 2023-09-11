using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    #region character attributes
    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;

    public float regenRate; //paused if dot ticks
    [HideInInspector] public float tempRegen;

    public float maxSpeed;
    public float currentSpeed;

    public float maxArmour;
    public float currentArmour;

    public float maxBaseAttackDamage;
    public float currentBaseAttackDamage;

    public bool immune;
    public bool isDead;

    public bool pushable;

    public bool isWet;

    [Header("Resistances")]
    public StatSystem.Resistance Physical;
    public StatSystem.Resistance Fire;
    public StatSystem.Resistance Water;
    public StatSystem.Resistance Wind;
    public StatSystem.Resistance Earth;
    public StatSystem.Resistance Light;
    public StatSystem.Resistance Dark;
    #endregion

    public Dictionary<string, StatSystem.Resistance> resistances = new Dictionary<string, StatSystem.Resistance>();

    public int statusEffectStacks = 0;
    public UnityEvent OnDamaged;
    public UnityEvent OnDeath;
    public UnityEvent OnBuffRecieved;

    private void Awake()
    {
        resistances.Add("Physical", Physical);
        resistances.Add("Fire", Fire);
        resistances.Add("Water", Water);
        resistances.Add("Wind", Wind);
        resistances.Add("Earth", Earth);
        resistances.Add("Light", Light);
        resistances.Add("Dark", Dark);
        currentHealth = maxHealth;
        currentArmour = maxArmour;
        currentBaseAttackDamage = maxBaseAttackDamage;
        tempRegen = regenRate;
        currentSpeed = maxSpeed;
    }

    #region Damage
    public void TakeDamage(float damage)
    {
        Debug.Log("recieved" + damage);
        currentHealth -= damage;
        OnDamaged.Invoke();
        StartCoroutine(ToggleImmunity());
        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            Die();
        }
    }
    public void Die()
    {
        if (gameObject.tag == "Enemy")
        {
            LevelManager.enemiesKilled++;
            if (LevelManager.enemiesKilled > LevelManager.totalEnemiesInLevel)
                LevelManager.enemiesKilled = LevelManager.totalEnemiesInLevel;
            if (LevelManager.instance != null) LevelManager.instance.UpdateUI();
        }
        OnDeath.Invoke();
    }
    public IEnumerator ToggleImmunity()
    {
        immune = true;
        yield return new WaitForSeconds(0.5f);
        immune = false;
        yield return null;

    }
    #endregion

}
