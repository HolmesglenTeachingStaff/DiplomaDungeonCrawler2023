using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Otter_Healer_FiniteStateMachine_Test : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Roaming,
        Healing,
        BackToHome,
        WaitingToHeal,
        LookingAround
    }

    public NPCState currentState = NPCState.Idle;
    public float roamRadius = 10f;
    public float backToHomeDistance = 15f;
    public Transform homeLocation;
    public float healDistance = 2f;

    private NavMeshAgent navMeshAgent;
    //private PlayerHealth playerHealth;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        // Start the FSM
        InvokeRepeating("StateMachine", 0f, 1f); // Call StateMachine every second
    }

    void StateMachine()
    {
        switch (currentState)
        {
            case NPCState.Idle:
                // Implement Idle state logic (stand still)
                break;

            case NPCState.Roaming:
                // Implement Roaming state logic (go to random locations)
                Roam();
                break;

            case NPCState.Healing:
                // Implement Healing state logic (access health script when player presses E)
                //TryHealPlayer();
                break;

            case NPCState.BackToHome:
                // Implement BackToHome state logic (go back to a defined location)
                GoBackToHome();
                break;

            case NPCState.WaitingToHeal:
                // Implement WaitingToHeal state logic (waiting for player interaction)
                break;

            case NPCState.LookingAround:
                // Implement LookingAround state logic (stop and gaze for a few seconds)
                StartCoroutine(LookAround());
                break;
        }
    }

    void Roam()
    {
        // Implement Roaming logic
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas);
            navMeshAgent.SetDestination(hit.position);
        }
    }

    //void TryHealPlayer()
    //{
        // Implement Healing logic
        //float distanceToPlayer = Vector3.Distance(transform.position, playerHealth.transform.position);
        //if (distanceToPlayer <= healDistance && Input.GetKeyDown(KeyCode.E))
        //{
            // Heal the player (you need to implement your own healing logic)
          //  playerHealth.Heal();
        //}
    //}

    void GoBackToHome()
    {
        // Implement BackToHome logic
        float distanceToHome = Vector3.Distance(transform.position, homeLocation.position);
        if (distanceToHome > backToHomeDistance)
        {
            navMeshAgent.SetDestination(homeLocation.position);
        }
        else
        {
            currentState = NPCState.WaitingToHeal;
        }
    }

    IEnumerator LookAround()
    {
        // Implement LookingAround logic
        currentState = NPCState.Idle; // Stop moving
        yield return new WaitForSeconds(3f); // Look around for 3 seconds
        currentState = NPCState.Roaming; // Go back to Roaming state
    }
}
