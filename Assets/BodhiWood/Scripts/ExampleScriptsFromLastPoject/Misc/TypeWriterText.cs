using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterText : MonoBehaviour
{
    //Get a reference to the text UI.
    public TMP_Text textUI;

    //Hold a collection of messages.
    public string[] messages;

    //Customise timing
    public float letterDelay;
    public float messageDelay;

    void Start()
    {
        //Run the TypeWriter Coroutine.
        StartCoroutine(TypeWriter());
    }

    IEnumerator TypeWriter()
    {
        //Select the first message.
        textUI.text = "";

        //Create a loop that runs through every message.
        for (int i = 0; i < messages.Length; i++)
        {
            //In the loop, create a array of every character in the message.
            char[] chars = messages[i].ToCharArray();

            //Loop through each character in the character array.
            for(int j = 0; j < chars.Length; j++)
            {
                //For each Character, add it to a string, pause, then update the ui with the new string.
                textUI.text += chars[j].ToString();
                yield return new WaitForSeconds(letterDelay);
                //When all characters are displayed on the string, end the loop.
            }
            //When the character loop is finished, pause, then wipe the message, then move to the next message and repeat the loop.
            yield return new WaitForSeconds(messageDelay);
            textUI.text = "";
        }

        //When all messages are complete, end the coroutine.
        yield return null;
    }
}
