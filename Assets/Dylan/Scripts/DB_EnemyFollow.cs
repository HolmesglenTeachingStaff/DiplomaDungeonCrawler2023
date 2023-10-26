using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DB_EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public float speed;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>();
        GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(player.position * Time.deltaTime);
    }
}
