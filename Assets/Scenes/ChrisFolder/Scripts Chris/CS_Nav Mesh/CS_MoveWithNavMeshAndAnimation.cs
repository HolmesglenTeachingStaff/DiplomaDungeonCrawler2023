using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_MoveWithNavMeshAndAnimation : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;

    Vector2 velocity = Vector2.zero;
    Vector2 smoothDeltaPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("IsMoving", shouldMove);
        anim.SetFloat("Horizontal", velocity.x);
        anim.SetFloat("Vertical", velocity.y);

        //GetComponent<CS_LookAtPos>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }
    private void OnAnimatorMove()
    {
        transform.position = agent.nextPosition;
    }
}