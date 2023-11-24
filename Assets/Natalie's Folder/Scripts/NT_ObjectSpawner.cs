using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NT_ObjectSpawner : MonoBehaviour
{
    public bool isActive;
    public float spawnTime;
    private float lastSpawnTime = 0;

    public GameObject objectPrefab;
    public Transform spawnPoint;
   

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
