using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritHandler : MonoBehaviour
{
    public float spiritPower;
    bool isDepleted;

    [SerializeField] ParticleSystem spiritParticle;

    public void ReleaseSpirit()
    {
        if (isDepleted) return;
        ParticleSystem particle = Instantiate(spiritParticle.gameObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        particle.Play();
        LevelManager.instance.UpdateSpirit(spiritPower);
        isDepleted = true;
    }
}
