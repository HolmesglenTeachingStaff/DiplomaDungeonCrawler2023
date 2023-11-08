using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NT_DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private NT_DialogueObject testDialogue;

    //public TextMeshProUGUI npcName;
    private NT_ResponseHandler responseHandler;
    private NT_TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<NT_TypeWriterEffect>();
        responseHandler = GetComponent<NT_ResponseHandler>();

        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(NT_DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    
    private IEnumerator StepThroughDialogue(NT_DialogueObject dialogueObject)
    {
        //foreach(string dialogue in dialogueObject.Dialogue)
        //{
            //yield return typeWriterEffect.Run(dialogue, textLabel);
            //yield return new WaitUntil(() => Input.GetMouseButton(0));
        //}

        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;
            yield return new WaitUntil(() => Input.GetMouseButton(0));
        }

        if (dialogueObject.HasResponses)
        {
            //NT_ResponseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
        
    }
    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
