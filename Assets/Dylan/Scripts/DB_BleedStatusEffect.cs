using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_BleedStatusEffect : MonoBehaviour
{
    private Stats healthStat;

    public List<int> bleedTickTimers = new List<int>();

    void Start()
    {
        healthStat = GetComponent<Stats>();
    }

    public void ApplyBleed(int ticks)
    {
        if(bleedTickTimers.Count > 0)
        {
            bleedTickTimers.Add(ticks);
            StartCoroutine(Bleed());
        }
        else
        {
            bleedTickTimers.Add(ticks);
        }
    }

    IEnumerator Bleed()
    {
        while(bleedTickTimers.Count > 0)
        {
            for(int i = 0; i < bleedTickTimers.Count; i++)
            {
                bleedTickTimers[i]--;
            }
            healthStat.currentHealth -= 5;
            bleedTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
