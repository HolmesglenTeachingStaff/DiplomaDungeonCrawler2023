using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AreaPricer : MonoBehaviour
{
    NavMeshAgent agent;

    //area costs
    public float walkable;
    public float fire;
    public float mud;
    public float secret;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetAreaCost(0, walkable);
        agent.SetAreaCost(3, fire);
        agent.SetAreaCost(4, mud);
        agent.SetAreaCost(5, secret);
    }
}
