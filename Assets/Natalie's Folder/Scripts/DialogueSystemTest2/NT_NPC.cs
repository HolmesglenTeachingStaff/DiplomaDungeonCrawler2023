using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC file", menuName = "NPC files archive")]
public class NT_NPC : ScriptableObject
{
    public string npcName;
    [TextArea(3, 30)]
    public string[] dialogue;
    [TextArea(3, 30)]
    public string[] playerDialogue;
}
