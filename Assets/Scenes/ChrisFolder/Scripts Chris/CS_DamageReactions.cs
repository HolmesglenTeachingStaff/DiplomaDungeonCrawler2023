using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DamageReactions : MonoBehaviour
{   

    public float damageAmount;
    public float chargeDamage;
    public StatSystem.DamageType damageType; //refferencing another script (StatSystem)

    public float damageRange;

    public LayerMask attackableLayers; //for enabling layer to be effected


    public void DamageBurst()
    {
        //get objects within radius and store to variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers); //creating array for every collider the sphere connects with in a layer mask
        //itterate through each potential target
        if(possibleEnemies.Length > 0)
        {
            foreach(Collider enemy in possibleEnemies)
            {   //check the target isn't me, this is to stop looping damaging again
                if(enemy.gameObject != gameObject)//if not targetting myself
                {
                    //check the target has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                     //deal damage to Stat
                     if(stats != null)
                     StatSystem.DealDamage(stats, damageType, damageAmount);
                }
            }
        }
        
        
    }

    public void ChargeBurst()
    {
        //get objects within radius and store to variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers); //creating array for every collider the sphere connects with in a layer mask
        //itterate through each potential target
        if(possibleEnemies.Length > 0)
        {
            foreach(Collider enemy in possibleEnemies)
            {   //check the target isn't me, this is to stop looping damaging again
                if(enemy.gameObject != gameObject)//if not targetting myself
                {
                    //check the target has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                     //deal damage to Stat
                     if(stats != null)
                     StatSystem.DealDamage(stats, damageType, chargeDamage);
                }
            }
        }
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
