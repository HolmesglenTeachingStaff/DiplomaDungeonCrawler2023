using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_StatSystem : MonoBehaviour
{   
    /*public StatSystem.DamageType damageType;
    public float damageAmount;
    public float chargeDamage;
    public float damageRange;

    public bool isAttackable;
    public bool isChargable;

    public LayerMask attackableLayers; //for enabling layer to be effected

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Awake()
    {       
        public bool isAttackable = false;
        public bool isChargable = false;
    }

    public void DamageMelee()
    {
        if (isAttackable = false) yield return null; //if we can't attack them, don't attack them

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

    public void DamageCharge()
    {
        if (isChargable = false) yield return null; //if we can't charge them, don't charge them

        //get objects within radius and store to variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, damageRange, attackableLayers); //creating array for every collider the sphere connects with in a layer mask
        //itterate through each potential target
        if(possibleEnemies.Length > 0)
        {
            
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

    // Update is called once per frame
    void Update()
    {
        
        
    }*/
}
