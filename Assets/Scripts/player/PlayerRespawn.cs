using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    //reference to all scripts and objects that are turned off

    public GameObject gib;
    public GameObject playerModel;
    public Stats playerStats;

    public List<GameObject> gibs;
    public List<Vector3> gibPositions;
    //reference to gibs object
    //list of gibe subObjects
    //list of possitions

    // Start is called before the first frame update
    void Start()
    {
        //store all gibs in a list and all starting positions in another list using a foreachloop
        foreach(Transform gib in gib.transform)
        {
            gibs.Add(gib.gameObject);
            gibPositions.Add(gib.localPosition);
        }
    }

    public void Respawn()
    {
        //loop through the gib objects, resetting their positions and velocity
        for (int i = 0; i < gibs.Count; i++)
        {
            var rb = gibs[i].GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                gibs[i].transform.localPosition = gibPositions[i];
            }
            gib.SetActive(false);
        }
        playerModel.SetActive(true);
        //gameObject.GetComponent<PlayerMovment>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
