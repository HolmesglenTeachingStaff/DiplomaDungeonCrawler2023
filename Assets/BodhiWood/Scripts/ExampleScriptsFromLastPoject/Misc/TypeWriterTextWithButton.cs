using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterTextWithButton : MonoBehaviour
{
    //Get a reference to the text UI.
    public TMP_Text textUI;

    //Hold a collection of messages.
    public string[] messages;

    //Customise timing
    public float letterDelay;

    int messageCounter = 0;

    void Start()
    {
        //Run the TypeWriter Coroutine.
        StartCoroutine(TypeWriter());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && messageCounter < messages.Length -1)
        {
            messageCounter += 1;
            StopAllCoroutines();
            StartCoroutine(TypeWriter());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && messageCounter > 0)
        {
            messageCounter -= 1;
            StopAllCoroutines();
            StartCoroutine(TypeWriter());
        }
    }

    IEnumerator TypeWriter()
    {
        //Select the first message.
        textUI.text = "";

        //In the loop, create a array of every character in the message.
        char[] chars = messages[messageCounter].ToCharArray();

        //Loop through each character in the character array.
        for (int j = 0; j < chars.Length; j++)
        {
            //For each Character, add it to a string, pause, then update the ui with the new string.
            textUI.text += chars[j].ToString();
            yield return new WaitForSeconds(letterDelay);
            //When all characters are displayed on the string, end the loop.
        }

        //When all messages are complete, end the coroutine.
        yield return null;
    }
}
