using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWayPoints : MonoBehaviour
{
    public List<Vector3> waypoints = new List<Vector3>();
    public bool willPatrol;
    public int currentWaypoint = 0;
    private void Start()
    {
        if (willPatrol) collectWaypoints();
    }
    private void collectWaypoints()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
            {
                waypoints.Add(child.position);
                Destroy(child.gameObject);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (waypoints.Count > 0)

        {
            foreach (Vector3 child in waypoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(child, 1);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Waypoint"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(child.position, 0.5f);
                }
            }
        }
    }
}
