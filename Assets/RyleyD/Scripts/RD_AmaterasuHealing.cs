using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RD_AmaterasuHealing : MonoBehaviour
{
    //store how much to heal

    public float healAmount = 10f;
    public float newMax = 200f;

    [SerializeField] VisualEffect burstPrefab;

    //get a reference to the GameObject
    private void OnTriggerStay(Collider other) //changed from enter to stay so is repetively called while inside the zone instead of only once.
    {
        if (Input.GetKeyDown(KeyCode.E))
            //check if the object has the correct tag to be a target
            if (other.tag == "Player")
            {
            //create a variable to hold the objects Stats script
            Stats statsScript = other.GetComponent<Stats>();

            //Adjust the variables in the stats script as you like.
            statsScript.currentHealth += healAmount;
            statsScript.maxHealth += statsScript.maxHealth * 0.5f;
            statsScript.maxHealth *= 1.5f;
            statsScript.OnBuffRecieved.Invoke(); //run the Onbuffrecieved event so that the UI can change in response when that feature is implemeted.
            //spawn the particle
            SpawnParticle();
            }
    }
    void SpawnParticle()
    {
        //istantiate a new particle
        VisualEffect newBurstEffect = Instantiate(burstPrefab, transform.position, transform.rotation);
        //play the particle
        newBurstEffect.Play();
        //destroy the particle
        Destroy(newBurstEffect.gameObject, 5f);
    }

}
