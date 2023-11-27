using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderlingManager : MonoBehaviour
{
    public GameObject spiderlingPrefab;
    public int maxSpiderlings = 5;
    public float spawnInterval = 5f; //how long is waited before trying to spawn another spiderling

    private List<Spiderling> spiderlingsList = new List<Spiderling>();


    void Start()
    {
        StartCoroutine(SpawnMinions());
    }

    IEnumerator SpawnMinions()
    {
        //continually try to spawn minions while below minion count
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spiderlingsList.Count < maxSpiderlings)
            {
                SpawnSpiderling();
            }
        }
    }

    public List<Spiderling> GetSpiderlings()
    {
        return spiderlingsList;
    }

    void SpawnSpiderling()
    {
        Vector3 spawnPosition = transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            // Instantiate spiderling at the valid position
            GameObject spiderling = Instantiate(spiderlingPrefab, hit.position, Quaternion.identity);
            Spiderling spiderlingScript = spiderling.GetComponent<Spiderling>(); //getting its 'spiderling' component
            spiderlingScript.SetBroodMother(this); //making this jorogumo it's "broodmother" (AKA to follow)
            spiderlingsList.Add(spiderlingScript); //adding it to spiderling list 

        }
        else
        {
            // Handle the case where a valid position couldn't be found (optional)
            Debug.LogError("Failed to find a valid position on the NavMesh for the spiderling.");
        }


    }

    //when spiderlings die, remove them from list
    public void SpiderlingDied(Spiderling spiderling)
    {
       spiderlingsList.Remove(spiderling);

    }
}
