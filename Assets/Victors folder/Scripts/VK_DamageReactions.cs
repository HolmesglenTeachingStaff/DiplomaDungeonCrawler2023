using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VK_DamageReactions : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;
    public float damageRange;
    public LayerMask attackableLayers;

    public void DamageBurst()
    {
        //get objects within a radius and store to a variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers);
        //itterate through each potential target
        if (possibleEnemies.Length > 0)
        {
            foreach(Collider enemy in possibleEnemies)
            {
                //check the target is not me
                if (enemy.gameObject != gameObject)
                {
                    //check the target has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                    //deal damage to Stat
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
