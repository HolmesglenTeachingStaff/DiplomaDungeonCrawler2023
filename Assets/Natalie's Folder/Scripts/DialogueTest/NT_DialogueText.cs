using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NT_DialogueText : MonoBehaviour
{
    public TMP_Text textUI;
    public string[] introMessages;
    public string[] loreMessages;
    public string[] noHealMessage;
    public string[] whoMessage;
    public string[] whatMessage;
    public string[] whyMessage;

    public float letterDelay;
    int messageCounter;
    [SerializeField] float messageCount;
    public bool HasDisplayed = false;
    public GameObject dialogueBox;
    public GameObject playerChar;

    //Variables for buttons  
    public Button healingButton;
    public Button whoButton;
    public Button whatButton;
    public Button whyButton;

    // Update is called once per frame
    void Update()
    {
        //Checks if button has been clicked 
        if (Input.GetKeyDown(KeyCode.E) && HasDisplayed == false)
        {
            messageCount++;
        }

        if(messageCount > 0)
        {
            HasDisplayed = true;
        }

        if(messageCount >= 2)
        {
            messageCount = 1;
        }

        if (Input.GetMouseButton(0) && messageCounter < introMessages.Length - 1 && messageCount == 0)
        {
            messageCounter += 1;
            StopAllCoroutines();
            StartCoroutine(TypeWriterIntro());
        }
        else if(Input.GetMouseButton(0) && messageCounter < introMessages.Length - 1 && messageCount >= 1)
        {
            messageCounter += 1;
            StopAllCoroutines();
            StartCoroutine(TypeWriterLore());
        }
    }

    public IEnumerator TypeWriterIntro()
    {

        for (int i = 0; i < introMessages.Length; i++)
        {
            char[] chars = introMessages[i].ToCharArray();

            for(int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;

        healingButton.enabled = true;
        whatButton.enabled = true;
        whoButton.enabled = true;
        whyButton.enabled = true;

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator TypeWriterLore()
    {
        textUI.text = "";

        for (int i = 0; i < loreMessages.Length; i++)
        {
            char[] chars = loreMessages[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;

        healingButton.enabled = true;
        whatButton.enabled = true;
        whoButton.enabled = true;
        whyButton.enabled = true;

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator NoHealingText()
    {
        textUI.text = "";

        for (int i = 0; i < noHealMessage.Length; i++)
        {
            char[] chars = noHealMessage[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;
    }

    public IEnumerator Healing()
    {
        if (playerChar.GetComponent<Stats>().currentHealth <= 100)
        {
            //Plays healing animation with shader
            playerChar.GetComponent<Stats>().currentHealth = 100;
        }
        else if (playerChar.GetComponent<Stats>().currentHealth >= 100)
        {
            StartCoroutine(NoHealingText());
        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator WhoDialogue()
    {
        textUI.text = "";

        for (int i = 0; i < whoMessage.Length; i++)
        {
            char[] chars = whoMessage[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;
    }

    public IEnumerator WhatDialogue()
    {
        textUI.text = "";

        for (int i = 0; i < whatMessage.Length; i++)
        {
            char[] chars = whatMessage[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;
    }

    public IEnumerator WhyDialogue()
    {
        textUI.text = "";

        for (int i = 0; i < whyMessage.Length; i++)
        {
            char[] chars = whyMessage[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;
    }
}
