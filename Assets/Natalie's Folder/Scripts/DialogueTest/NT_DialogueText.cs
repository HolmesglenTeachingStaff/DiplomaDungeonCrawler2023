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
    public string[] whyAmIHere;
    public string[] continueMessage;
    public string[] healingMessage;

    public float letterDelay;
    int messageCounter;
    [SerializeField] float messageCount;
    public bool HasDisplayed = false;
    public bool canTalk = false;
    public GameObject dialogueBox;
    public GameObject playerChar;
    private Animator anim;

    //Variables for buttons  
    public GameObject healingButton;
    public GameObject whoButton;
    public GameObject whatButton;
    public GameObject whyButton;
    public GameObject whyAmIHereButton;
    public GameObject endButton;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Checks for canTalk variable 
        if(canTalk == true)
        {
            //Checks if button has been clicked 
            if (Input.GetKeyDown(KeyCode.E) && HasDisplayed == false)
            {
                messageCount++;
                StopAllCoroutines();
                StartCoroutine(TypeWriterIntro());
                dialogueBox.gameObject.SetActive(true);
                healingButton.gameObject.SetActive(false);
                whatButton.gameObject.SetActive(false);
                whoButton.gameObject.SetActive(false);
                whyButton.gameObject.SetActive(false);
                endButton.gameObject.SetActive(false);
                whyAmIHereButton.gameObject.SetActive(false);
            }

            if(Input.GetKeyDown(KeyCode.E) && HasDisplayed == true)
            {
                StopAllCoroutines();
                StartCoroutine(TypeWriterLore());
                dialogueBox.gameObject.SetActive(true);
            }
        }

        if (messageCount > 0)
        {
            HasDisplayed = true;
        }

        if (messageCount >= 2)
        {
            messageCount = 1;
        }
    }

    public IEnumerator TypeWriterIntro()
    {
        anim.Play("NT_TsukuyomiWavingAnimation");
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
        dialogueBox.gameObject.SetActive(false);
        yield return null;

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator TypeWriterLore()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiWavingAnimation");

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

        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator NoHealingText()//Not showing the dialogue when full health 
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

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

        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Healing()
    {
        if (playerChar.GetComponent<Stats>().currentHealth < playerChar.GetComponent<Stats>().maxHealth)
        {
            //Plays healing animation
            //anim.Play("NT_TsukuyomiHealingAnimation");
            playerChar.GetComponent<Stats>().currentHealth = playerChar.GetComponent<Stats>().maxHealth;

            textUI.text = "";

            for (int i = 0; i < healingMessage.Length; i++)
            {
                char[] chars = healingMessage[i].ToCharArray();

                for (int j = 0; j < chars.Length; j++)
                {
                    textUI.text += chars[j].ToString();
                    yield return new WaitForSeconds(letterDelay);
                }
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                textUI.text = "";
            }
            yield return null;

            healingButton.gameObject.SetActive(true);
            whatButton.gameObject.SetActive(true);
            whoButton.gameObject.SetActive(true);
            whyButton.gameObject.SetActive(true);
            endButton.gameObject.SetActive(true);
            whyAmIHereButton.gameObject.SetActive(true);
        }
        else 
        {
            StartCoroutine(NoHealingText());
        }
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator WhoDialogue()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

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

        StartCoroutine(ContinueDialogue());
        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator WhatDialogue()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

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

        StartCoroutine(ContinueDialogue());
        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator WhyDialogue()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

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

        StartCoroutine(ContinueDialogue());
        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator WhyAmIHereQues()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

        for (int i = 0; i < whyAmIHere.Length; i++)
        {
            char[] chars = whyAmIHere[i].ToCharArray();

            for (int j = 0; j < chars.Length; j++)
            {
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            textUI.text = "";
        }
        yield return null;

        StartCoroutine(ContinueDialogue());
        healingButton.gameObject.SetActive(true);
        whatButton.gameObject.SetActive(true);
        whoButton.gameObject.SetActive(true);
        whyButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(true);
        whyAmIHereButton.gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator ContinueDialogue()
    {
        textUI.text = "";
        anim.Play("NT_TsukuyomiHandGestureAnimation");

        for (int i = 0; i < continueMessage.Length; i++)
        {
            char[] chars = continueMessage[i].ToCharArray();

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

    #region Functions for onclickevent
    public void EndDialogue()
    {
        dialogueBox.gameObject.SetActive(false);
        healingButton.gameObject.SetActive(false);
        whatButton.gameObject.SetActive(false);
        whoButton.gameObject.SetActive(false);
        whyButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);
        whyAmIHereButton.gameObject.SetActive(false);
    }
    
    public void CanGetHeal()
    {
        StopAllCoroutines();
        StartCoroutine(Healing());
    }

    public void WhyDialogueChoice()
    {
        StopAllCoroutines();
        StartCoroutine(WhyDialogue());
    }

    public void WhatDialogueOption()
    {
        StopAllCoroutines();
        StartCoroutine(WhatDialogue());
    }

    public void WhoDialogueOption()
    {
        StopAllCoroutines();
        StartCoroutine(WhoDialogue());
    }

    public void WhyAmIHereQ()
    {
        StopAllCoroutines();
        StartCoroutine(WhyAmIHereQues());
    }
    #endregion
}
