using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ObjectSpawner : MonoBehaviour
{

    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnTime;
    private float lastSpawnTime;
    public bool isActive;

    

    // Update is called once per frame
    void Update()
    {
        if(!isActive) return;

        if(Time.time - lastSpawnTime >= spawnTime) //Time.time = games elapsed time, lastSpawnTime = last time missile spawned
        {
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation); //instantiating the prefab (bullet)
            lastSpawnTime = Time.time; //adds to the timer counter
        }
    }
}
