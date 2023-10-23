using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CS_TypeWriter : MonoBehaviour
{
    //get a refference to the text UI
    public TMP_Text textUI;

    //hold a collection of messages
    public string[] messages;

    //customize timing, these below will be used to customize wait times between lettering or messages appearing
    public float letterDelay;
    public float messageDelay;

    // Start is called before the first frame update
    void Start()
    {
        //run the typewriter coroutine
        StartCoroutine(TipeWriter());
    }

    IEnumerator TipeWriter()
    {
        //select the first message
        textUI.text = ""; //this makes the initial message nothing as a place holder

        //create a loop that runs through every message
        for(int i = 0; i < messages.Length; i ++)
        {     

        //in the loop create an array of every character in the message
        char[] chars = messages[i].ToCharArray();

        //loop through each character in the character array
        for(int j = 0; j < chars.Length; j++)
        {        
        //for each character add it to a string, pause, then update UI with a new string
        textUI.text += chars[j].ToString();
        yield return new WaitForSeconds(letterDelay);

        //when all characters are displayed on the screen end the loop
        }
        
        //when the character loop is ended, pause, then wipe the message, then move to the next message and repeat the loop
        yield return new WaitForSeconds(messageDelay); //the timer chosen above in "messageDelay"
        textUI.text = ""; //this is to give nothing more, in order to wait for the next message
        }

        //when all messages are complete, end the corouting

        yield return null;
    }

}
