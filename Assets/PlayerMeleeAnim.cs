using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAnim : StateMachineBehaviour
{
    [SerializeField]
    float minDistance;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<PlayerMovment>().canMove = false;
       // animator.gameObject.GetComponentInParent<PlayerMovment>().shouldLook = false;
        animator.gameObject.GetComponent<PlayerIKHandling>().ikActive = false;
       // damageCollider.enabled = true;
        animator.applyRootMotion = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovment pMove = animator.gameObject.GetComponentInParent<PlayerMovment>();
        pMove.moveSpeed = pMove.defaultMoveSpeed * 0.5f;

        pMove.moveDirection = pMove.lookTarget;
        

        animator.transform.parent.position = animator.rootPosition;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovment pMove = animator.gameObject.GetComponentInParent<PlayerMovment>();
        pMove.canMove = true;
       // pMove.shouldLook = true;
        pMove.moveSpeed = pMove.defaultMoveSpeed;
        animator.gameObject.GetComponent<PlayerIKHandling>().ikActive = true;
        animator.applyRootMotion = false;
    }
    
}

