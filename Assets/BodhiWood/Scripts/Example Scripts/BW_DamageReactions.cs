using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_DamageReactions : MonoBehaviour
{
    public float damageAmount;
    public StatSystem.DamageType damageType;
    public float damageRange;

    public LayerMask damageableLayers;

    public void DamageBurst()
    {
        //Get object within a radius and store to a variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position + Vector3.up, damageRange, damageableLayers);
        //Itterate through each potential target
        if (possibleEnemies.Length > 0)
        {
            foreach (Collider enemy in possibleEnemies)
            {
                //Check the target is not me
                if (enemy.gameObject != gameObject)
                {
                    //Check the target has the Stat script
                    Stats stats = enemy.GetComponent<Stats>();
                    //Deal damage to the Stat script
                    if (stats != null)
                    {
                        StatSystem.DealDamage(stats, damageType, damageAmount);
                    }
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
        Gizmos.DrawWireSphere(transform.position + Vector3.up, damageRange);
    }
}
