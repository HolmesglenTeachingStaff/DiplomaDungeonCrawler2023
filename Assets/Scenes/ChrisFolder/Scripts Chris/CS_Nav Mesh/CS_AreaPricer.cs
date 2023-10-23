using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_AreaPricer : MonoBehaviour //this script is to be able to change baked NavMeshAgent area costs, making it so npcs' can choose to take secret routes or walk over fire etc
{ //remember to add the new walkable areas to the agents (add fire, mud, secret for eg)
    public NavMeshAgent agent;

    public float walkable;
    public float fire;
    public float mud;
    public float secret;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetAreaCost(0, walkable);
        agent.SetAreaCost(4, fire);
        agent.SetAreaCost(5, mud);
        agent.SetAreaCost(6, secret);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
