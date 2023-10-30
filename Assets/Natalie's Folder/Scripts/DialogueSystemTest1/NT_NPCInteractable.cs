using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NT_NPCInteractable : MonoBehaviour
{
    public NT_Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<NT_DialogueManager>().StartDialogue(dialogue);
    }
}
