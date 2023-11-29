using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class RD_NinjaSmoleBomb : MonoBehaviour
{
    [SerializeField] VisualEffect smokeBombEffect;
    [SerializeField] VisualEffect burstPrefab;



    // Start is called before the first frame update
    private void Start()
    {
        //PlayParticle();
        SpawnParticle();
    }

    // Update is called once per frame
    /*
    void PlayParticle()
    {
        smokeBombEffect.Play();
    }
    */

    //CHECK IF THE HEALTH IS LOW ENOUGH
    public void CheckToPlayParticle()
    {
        //get a reference to health
        Stats healthScript = GetComponent<Stats>();

        //check health
        if(healthScript.currentHealth < (healthScript.maxHealth * 0.25f))
        {
            //spawn the particle
            SpawnParticle();

            //if you want to, you could change the states in your statemachine in this function too
        }
    }
    void SpawnParticle()
    {
        //istantiate a new particle
        VisualEffect newBurstEffect = Instantiate(burstPrefab, transform.position, transform.rotation);
        //play the particle
        newBurstEffect.Play();
        //destroy the particle
        Destroy(newBurstEffect.gameObject, 20f);
    }


}
