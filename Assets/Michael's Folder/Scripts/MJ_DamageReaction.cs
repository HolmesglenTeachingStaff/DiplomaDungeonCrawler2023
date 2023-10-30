using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJ_DamageReaction : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;
    public float damageRange;

    public LayerMask attackableLayers;

    public void DamageBurst()
    {
        //get objects within radius and store to variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, attackableLayers);
        //iterate through each target
        if (possibleEnemies.Length > 0)
        {
            foreach (Collider enemy in possibleEnemies)
            {
                if (enemy.gameObject != gameObject)
                {
                    //check if target is not me and has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                    //deal damage to stat
                    if (stats != null) StatSystem.DealDamage(stats, damageType, damageAmount);
                }
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
