using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NT_DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private NT_DialogueObject testDialogue;

    public TextMeshProUGUI npcName;
    private NT_TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<NT_TypeWriterEffect>();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(NT_DialogueObject dialogueObject)
    {
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    
    private IEnumerator StepThroughDialogue(NT_DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(2);
        foreach(string dialogue in dialogueObject.Dialogue)
        {
            yield return typeWriterEffect.Run(dialogue, textLabel); 
        }
    }
}
