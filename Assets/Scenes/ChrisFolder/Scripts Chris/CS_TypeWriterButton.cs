using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_TypeWriterButton : MonoBehaviour
{
    //Get a reference to the text UI
    public TMP_Text textUI;
    //Hold a collection of messages
    public string[] messages;

    //customise timing
    public float letterDelay;

    int messageCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Run the TypeWriter Coroutine
        StartCoroutine(TipeWriter());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && messageCounter < messages.Length - 1)
        {
            messageCounter += 1;
            StopAllCoroutines();
            StartCoroutine(TipeWriter());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && messageCounter > 0)
        {
            messageCounter -= 1;
            StopAllCoroutines();
            StartCoroutine(TipeWriter());
        }
    }

    IEnumerator TipeWriter()
    {
        //select the first message
        textUI.text = "";

        //in the loop create a array of every character in the message
        char[] chars = messages[messageCounter].ToCharArray();

        //loop through each character in the character array
        for (int j = 0; j < chars.Length; j++)
        {
             //for each character, add it to a string, pause, then update the ui with the new string
            textUI.text += chars[j].ToString();
            yield return new WaitForSeconds(letterDelay);
            //when all characters are dispayed on the string, end the loop
        }
        //when all messages are complete, end the coroutine.
        yield return null;
    }
}