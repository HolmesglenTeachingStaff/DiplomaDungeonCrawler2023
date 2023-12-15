using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Otter_Healer_NavAnimator : MonoBehaviour
{

    Animator otter_Anim;
    NavMeshAgent otter_Agent;

    Vector2 velocity = Vector2.zero;
    Vector2 smoothDeltaPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        otter_Anim = GetComponent<Animator>();
        otter_Agent = GetComponent<NavMeshAgent>();

        otter_Agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = otter_Agent.nextPosition - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);


    if(Time.deltaTime > 1e-5f)
        {
            velocity = smoothDeltaPosition / Time.deltaTime;
        }
    }

     

}
