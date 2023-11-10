using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DR_ElementalAttack : MonoBehaviour
{
    public float smallAttackRange, bigAttackRange;
    public StatSystem.DamageType damageType;

    public float minDamage, maxDamage;

    public float burstRate, lastBurst;

    public LayerMask attackableLayers;

    public bool attackActive;
    // Start is called before the first frame update
    void Start()
    {
        lastBurst = Time.time;
        attackActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && Time.time - lastBurst > burstRate && attackActive)
        {
            Explode(smallAttackRange, Random.Range(minDamage, maxDamage));
        }
    }
    public void Explode(float radius, float damage)
    {
        lastBurst = Time.time;
        if(radius == smallAttackRange)
        {
            GetComponentInParent<DR_Elemental_StateMachine>().particles.explode.Play();
        }
        //get objects within a radius and store to a variable
        Collider[] possibleEnemies = Physics.OverlapSphere(transform.position, radius, attackableLayers);
        
        //itterate through each potential target
        if (possibleEnemies.Length > 0)
        {
            foreach (Collider enemy in possibleEnemies)
            {
                //check the target is not me
                if (enemy.gameObject != gameObject)
                {
                    //check the target has Stat
                    Stats stats = enemy.GetComponent<Stats>();
                    //deal damage to Stat
                    if (stats != null)
                        StatSystem.DealDamage(stats, damageType, damage);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bigAttackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, smallAttackRange);
    }
}
