using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameObject spiderling = Instantiate(spiderlingPrefab, transform.position, Quaternion.identity); //creating spider
        Spiderling spiderlingScript = spiderling.GetComponent<Spiderling>(); //getting its 'spiderling' component
        spiderlingScript.SetBroodMother(this); //making this jorogumo it's "broodmother" (AKA to follow)
        //spiderlingScript.SetTarget(broodMother.transform);
        spiderlingsList.Add(spiderlingScript); //adding it to spiderling list 

    }

    //when spiderlings die, remove them from list
    public void SpiderlingDied(Spiderling spiderling)
    {
       spiderlingsList.Remove(spiderling);

    }
}
