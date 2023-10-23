using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_FollowTarget : MonoBehaviour
{

    public NavMeshAgent agent;

    public Transform targetObject;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetObject.position);
    }
}
