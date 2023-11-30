using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Otter_NavMesh : MonoBehaviour
{
    [SerializeField]
    private Transform movePisitionTransform;


    private NavMeshAgent otter_Nav;

    private void Awake()
    {
        otter_Nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        otter_Nav.destination = movePisitionTransform.position;


    }




}
