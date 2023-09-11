using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Takes in an array of prefabs (enemyTypes), an array of positions (spawnPositions) and randomly picks one enemy and 
/// and position to spawn it from.
/// Optionally this can also take a trigger zone to activate the spawner, as well as waypoints for AI that will patrol.
/// 
/// SET UP: 
/// This script should be on an empty object with a rigidbody set to isKinomatic & !useGravity.
/// all desired spawn points should be empty child game objects with the tag SpawnPoint
/// all waypoints (optional) should be empty child game objects with the tag Waypoint
/// all enemy type prefabs need to have a Health script attached to them
/// </summary>
public class SpawnerManager : MonoBehaviour
{
    [SerializeField] int totalEnemiesSpawned;
    [SerializeField] int maxEnemiesAtOnce;
    [SerializeField] int maxEnemies;

    [SerializeField] float spawnTime;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();
    private List<Vector3> waypoints = new List<Vector3>();

    [SerializeField] GameObject[] enemyTypes;

    [SerializeField] bool isEnabled;
    [SerializeField] bool enableOnTrigger;

    [SerializeField] Transform player;

    private void Start()
    {
        SetUpChildren();
        if (isEnabled)
        {
            InvokeRepeating("CheckToSpawn", spawnTime, spawnTime);
        }
    }
    void CheckToSpawn()
    {
        if(isEnabled && totalEnemiesSpawned < maxEnemies && currentEnemies.Count < maxEnemiesAtOnce)
        {
            var a = Random.Range(0, enemyTypes.Length); //select a random enemy
            var b = Random.Range(0, spawnPositions.Count); //select a random position

            //spawn the enemy at the location
            GameObject spawnedObject = Instantiate(enemyTypes[a], spawnPositions[b], enemyTypes[a].transform.rotation);
            currentEnemies.Add(spawnedObject);
            spawnedObject.GetComponent<ExampleAI>().target = player;

            //assign patrol points if desired.
            if(waypoints.Count > 0)
            {
                var spawnedScript = spawnedObject.GetComponent<AIWayPoints>();
                spawnedScript.waypoints = waypoints;
                spawnedScript.willPatrol = true;
            }
            totalEnemiesSpawned++;
        }
        else if(isEnabled && totalEnemiesSpawned >= maxEnemies)
        {
            Destroy(gameObject);
        }
        else
        {
            currentEnemies.RemoveAll(item => item == null);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isEnabled)
        {
            isEnabled = true;
            InvokeRepeating("CheckToSpawn", spawnTime, spawnTime);
        }
    }
    void SetUpChildren()
    {
        foreach(Transform child in transform)
        {
            if (child.tag == "SpawnPoint")
            {
                spawnPositions.Add(child.position);
            }
            if(child.tag == "Waypoint")
            {
                waypoints.Add(child.position);
            }
            if (child.CompareTag("SpawnerTrigger"))
            {
                if (enableOnTrigger)
                {
                    isEnabled = false;
                }
                else
                {
                    isEnabled = true;
                    Destroy(child.gameObject);
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(child.transform.position, 0.25f);
            }
            else if (child.CompareTag("SpawnPoint"))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawIcon(child.transform.position, "Spawn Point", false, Color.red);
            }
            else if (child.CompareTag("SpawnerTrigger"))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(child.transform.position, child.localScale);
            }
        }
    }
}
