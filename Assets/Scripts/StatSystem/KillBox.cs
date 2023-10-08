using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Stats stats = other.GetComponent<Stats>();
            if(stats != null)
            {
                stats.currentHealth = 0f;
                stats.currentArmour = 0f;
                stats.isDead = true;
                stats.OnDeath.Invoke();
            }
        }
    }
}
