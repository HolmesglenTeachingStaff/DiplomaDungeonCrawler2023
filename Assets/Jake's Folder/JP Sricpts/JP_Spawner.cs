using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_Spawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spwanTime;
    public float lastSpawnTime=0;
    public bool isActive;

    void Update()
    {
        if(!isActive)return;

        if(Time.time-lastSpawnTime>spwanTime)
        {
            Instantiate(objectPrefab,spawnPoint.position,spawnPoint.rotation);
            lastSpawnTime=Time.time;
        }
    }
}
