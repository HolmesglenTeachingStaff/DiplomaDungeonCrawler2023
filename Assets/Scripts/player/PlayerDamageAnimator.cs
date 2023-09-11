using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageAnimator : MonoBehaviour
{
    Animator anim;
    public enum attackPosition { behind, front, none}
    public attackPosition lastAttackPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastAttackPosition = attackPosition.none;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" || other.tag == "Hazard")
        {
            Vector3 directionToHit = transform.position - other.ClosestPoint(transform.position);
            float dy = Vector3.Dot(transform.forward, directionToHit);

            if (dy > 0f) lastAttackPosition = attackPosition.behind;
            if (dy < 0f) lastAttackPosition = attackPosition.front;
        }
    }
    public void PlayAnimation()
    {
        if(lastAttackPosition == attackPosition.behind) anim.Play("BackwardImpact");
        if (lastAttackPosition == attackPosition.front) anim.Play("forwardImpact");
    }
    public void PlayDeathAnimation()
    {
        if (lastAttackPosition == attackPosition.behind) anim.Play("backwardDeath");
        if (lastAttackPosition == attackPosition.front) anim.Play("forwardDeath");
        anim.SetBool("Dead", true);
    }
}
