using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RD_Healing : MonoBehaviour
{
    //store how much to heal

    public float healAmount = 10f;
    public float newMax = 200f;

    //get a reference to the GameObject
    private void OnTriggerEnter(Collider other)
    {
        //check if the object has the correct tag to be a target
        if (other.tag == "Player")
        {
            //create a variable to hold the objects Stats script
            Stats statsScript = other.GetComponent<Stats>();

            //Adjust the variables in the stats script as you like.
            statsScript.currentHealth += healAmount;
            statsScript.maxHealth += statsScript.maxHealth * 0.5f;
            statsScript.maxHealth *= 1.5f;
        }
    }


}
