using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_DamageReactions : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;

    public float damageRange;

    public LayerMask attackableLayers;

    public void DamageBurst()
    {
        //get objects withihn a radius and store to a variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers);
        //itterate through each potential target
        if(possibleEnemies.Length > 0)
        {
            foreach(Collider enemy in possibleEnemies)
            {
                //check the targrt is not me
                if(enemy.gameObject != gameObject)
                {
                    //check the traget has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                    //deal damage to Stat
                    if(stats != null)
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
