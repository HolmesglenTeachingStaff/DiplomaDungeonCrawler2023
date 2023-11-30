using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnTime;
    private float lastSpawnTime = 0;
    public bool isActive;

    void Update()
    {
        if (!isActive) return;

        if (Time.time - lastSpawnTime >= spawnTime)
        {
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
            lastSpawnTime = Time.time;
        }
    }
}
