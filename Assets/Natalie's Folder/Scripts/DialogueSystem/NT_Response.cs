using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NT_Response
{
    [SerializeField] private string responseText;
    [SerializeField] private NT_DialogueObject dialogueObject;

    public string ResponseText => responseText;

    public NT_DialogueObject DialogueObject => dialogueObject;
}
