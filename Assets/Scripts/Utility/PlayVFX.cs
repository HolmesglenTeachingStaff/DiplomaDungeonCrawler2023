using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{
    public VisualEffect[] visualEffects;
    public bool playByEnabling;
    private void Start()
    {
        if (playByEnabling)
        {
            foreach (VisualEffect item in visualEffects)
            {
                item.enabled = false;
            }
        }
    }
    void PlayEffect(int index)
    {
        if (playByEnabling) visualEffects[index].enabled = true;
        else visualEffects[index].Stop();
        StartCoroutine(StopEffect(index, 0.5f));
    }
    IEnumerator StopEffect(int index, float time)
    {
        yield return new WaitForSeconds(time);
        if (playByEnabling) visualEffects[index].enabled = false;
        else visualEffects[index].Stop();
        yield return null;
    }
}

