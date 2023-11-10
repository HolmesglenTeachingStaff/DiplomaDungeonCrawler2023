using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DR_ElementalPlayerTracker : MonoBehaviour
{
    public bool playerEntered;
    public bool playerExitted;

    // Start is called before the first frame update
    void Start()
    {
        playerEntered = false;
        playerExitted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerEntered == false && other.tag == "Player")
            playerEntered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (playerEntered == true && other.tag == "Player")
            playerExitted = true;
    }
}
