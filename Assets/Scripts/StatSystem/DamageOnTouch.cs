using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [Header("Damage Amount Settings")]
    [Tooltip("If true, the damage will be calculated from a baseAttackDamage stored on the attached Stats")]
    public bool damageIsBasedOnStats;
    [Tooltip("A stat script must be provided if damage is based on stats")]
    public Stats statBlock;
    [Space(10)]
    public float minDamage;
    public float maxDamage;
    [Space(10)]
    [Header("Damage Targeting")]
    public StatSystem.DamageType damageType;

    public enum DamageTargets { player, enemy, general}
    public DamageTargets damageTarget;

    [Space(10)]
    public GameObject particleEffect;

    private void OnTriggerEnter(Collider other)
    {
        //check target against damage target
        if(damageTarget == DamageTargets.player && other.tag == "Player" || damageTarget == DamageTargets.enemy && other.tag == "Enemy"
            || damageTarget == DamageTargets.general && other.tag == "Player" || damageTarget == DamageTargets.general && other.tag == "Enemy")
        {
            //make sure the target object is not itself
            if (other.gameObject == transform.parent || other.gameObject == transform.root) return;

            var targetStats = other.GetComponent<Stats>();
            Debug.Log("Hit");

            if (targetStats != null)
            {
                //determine damage amount
                float damage;
                if(damageIsBasedOnStats && statBlock != null)
                {
                    damage = Mathf.Clamp(statBlock.currentBaseAttackDamage, minDamage, maxDamage);
                }
                else
                {
                    damage = Random.Range(minDamage, maxDamage);
                }
                Debug.Log("attempted to deal" + damage + " " + damageType + " damage");

                if(damageType == StatSystem.DamageType.Wind || damageType == StatSystem.DamageType.Light)
                {
                    StatSystem.DealDamage(targetStats, damageType, damage, true, transform.position);
                }
                else
                {
                    StatSystem.DealDamage(targetStats, damageType, damage);
                }
                
                if (particleEffect)
                {
                    particleEffect.transform.position = other.ClosestPoint(transform.position);
                    particleEffect.GetComponent<ParticleSystem>().Play();
                }
                if (other.tag == "Enemy") CameraFeedback.instance.FramePause(0.25f);
                if (other.tag == "Player") CameraFeedback.instance.ScreenShake(0.05f);

            }
        }

        

    }
}
