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
