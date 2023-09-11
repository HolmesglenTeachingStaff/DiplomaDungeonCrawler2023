using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new quiver", menuName = "ScriptableObject/Quiver")]
public class Quiver : ScriptableObject
{
    public StatSystem.DamageType damageType;
    public int arrowCount;
    public int maxArrows;
    public GameObject arrow;
}
