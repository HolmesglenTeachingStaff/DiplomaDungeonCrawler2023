using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectScript : MonoBehaviour
{
    public string buffType;
    public float buffAmount;

    public enum TargetType { Player, Enemy, All };
    public TargetType targetType;

    private void OnTriggerStay(Collider other)
    {
        if(targetType == TargetType.All || targetType == TargetType.Player && other.tag == "Player" || targetType == TargetType.Enemy && other.tag == "Enemy")
        {
            var targetStats = other.GetComponent<Stats>();
            if (targetStats) StatSystem.BuffStat(targetStats, buffType, buffAmount);
            Debug.Log("attempt to heal");
        }
    }
}
