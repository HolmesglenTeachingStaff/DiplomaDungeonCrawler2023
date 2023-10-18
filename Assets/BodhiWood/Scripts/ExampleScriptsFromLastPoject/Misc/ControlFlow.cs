using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlFlow : MonoBehaviour
{
    GameObject[] objectsToSpawn;
    string[] objectNames;
    bool isRunning;
    int counter;

    void Start()
    {
        for(int i = 0; i < objectsToSpawn.Length; i++)
        {
            Instantiate(objectsToSpawn[i], transform.position, transform.rotation);
            objectNames[i] = objectsToSpawn[i].name;
            for (int j = 0; j < 10; j++)
            {

            }
        }

        foreach(string item in objectNames)
        {
            Debug.Log(item);
        }

        while (counter < 10)
        {
            counter++;
        }
    }

    void Update()
    {
        
    }
}
