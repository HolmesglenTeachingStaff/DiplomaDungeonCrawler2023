using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_DamegeReaction : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;
    public float damageRange;
    public LayerMask attackableLayers;

    public void DamageBurst()
    {
        Collider[] possibleEnemies=Physics.OverlapSphere(transform.position,damageRange,attackableLayers);
        if(possibleEnemies.Length>0)
        {
            foreach(Collider enemy in possibleEnemies)
            {
                if(enemy.gameObject!=gameObject)
                {
                    Stats stats=enemy.GetComponent<Stats>();
                    if(stats!=null)
                    {
                        StatSystem.DealDamage(stats,damageType,damageAmount);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,damageRange);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
