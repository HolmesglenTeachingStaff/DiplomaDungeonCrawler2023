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
        spiritParticle.Play();
        LevelManager.instance.UpdateSpirit(spiritPower);
        isDepleted = true;
    }
}
