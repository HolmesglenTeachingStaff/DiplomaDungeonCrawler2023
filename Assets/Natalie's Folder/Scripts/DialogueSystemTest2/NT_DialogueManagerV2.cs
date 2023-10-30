using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NT_DialogueManagerV2 : MonoBehaviour
{
    public NT_NPC npc;

    bool isTalking = false;

    float distance;
    float currentResponseTracker;

    public GameObject player;
    public GameObject dialogueUI;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI npcDialogueBox;
    public TextMeshProUGUI playerResponse;


    // Start is called before the first frame update
    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void OnMouseOver()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if(distance <= 2.5f)
        {
            if(Input.GetAxis("MouseScrollWheel") < 0f)
            {
                currentResponseTracker++;
                if(currentResponseTracker >= npc.playerDialogue.Length - 1)
                {
                    currentResponseTracker = npc.playerDialogue.Length - 1;
                }
            }
            else if (Input.GetAxis("MouseScrollWheel") < 0f)
            {
                currentResponseTracker--;
                if(currentResponseTracker < 0)
                {
                    currentResponseTracker = 0;
                }
            }
            //trigger dialogue
            if(Input.GetKeyDown(KeyCode.E) && isTalking == false)
            {
                StartConversation();
            }
            else if (Input.GetKeyDown(KeyCode.E) && isTalking == true)
            {
                EndDialogue();
            }

            if(currentResponseTracker == 0 && npc.playerDialogue.Length >= 0)
            {
                playerResponse.text = npc.playerDialogue[0];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    npcDialogueBox.text = npc.dialogue[1];
                }
            }
            else if (currentResponseTracker == 1 && npc.playerDialogue.Length >= 1)
            {
                playerResponse.text = npc.playerDialogue[1];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    npcDialogueBox.text = npc.dialogue[2];
                }
            }
            else if (currentResponseTracker == 2 && npc.playerDialogue.Length >= 2)
            {
                playerResponse.text = npc.playerDialogue[2];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    npcDialogueBox.text = npc.dialogue[3];
                }
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        currentResponseTracker = 0;
        dialogueUI.SetActive(true);
        npcName.text = npc.name;
        npcDialogueBox.text = npc.dialogue[0];
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
    }
}
