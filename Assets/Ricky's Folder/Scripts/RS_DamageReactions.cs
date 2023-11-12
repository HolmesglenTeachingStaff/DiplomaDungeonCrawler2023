using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_DamageReactions : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;

    public float damageRange;

    public LayerMask attackableLayers;

    public void DamageBurst()
    {
        // Get objects within a radius and store to a varriable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers);

        // Itterate through each potential target
        if(possibleEnemies.Length > 0)
        {
            foreach (Collider enemy in possibleEnemies)
            {
                // Check the target is not me
                if (enemy.gameObject != gameObject)
                {
                    // Check the target has stats
                    Stats stats = enemy.GetComponent<Stats>();

                    // Deal damage to stat
                    if (stats != null)
                    StatSystem.DealDamage(stats, damageType, damageAmount);
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
