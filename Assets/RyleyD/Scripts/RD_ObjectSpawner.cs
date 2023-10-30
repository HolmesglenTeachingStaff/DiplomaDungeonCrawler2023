using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RD_ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnTime;
    private float lastSpawnTime = 0;
    public bool isActive;


    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        if(Time.time - lastSpawnTime >= spawnTime)
        {
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
            lastSpawnTime = Time.time;
        }
    }
}
