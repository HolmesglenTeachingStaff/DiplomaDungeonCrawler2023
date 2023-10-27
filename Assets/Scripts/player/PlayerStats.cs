using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    public float spiritPower = 0;

    public void AddSoulPower(float spiritIncrease)
    {
        spiritPower += spiritIncrease;
        if(spiritPower >= 100)
        {
            LevelManager.spirits += 1;
            spiritPower -= 100;
            Mathf.Clamp(spiritPower, 0, 100);
        }
    }
}
