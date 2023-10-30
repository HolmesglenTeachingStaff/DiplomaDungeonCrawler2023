using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NT_TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    public void Run(string textToType, TMP_Text textLabel )
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        yield return new WaitForSeconds(2);

        float t = 0;
        int characterIndex = 0;

        while(characterIndex < textToType.Length)
        {
            t += Time.deltaTime *typeWriterSpeed;
            characterIndex = Mathf.FloorToInt(t);
            characterIndex = Mathf.Clamp(characterIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, characterIndex);         

            yield return null;
        }

        textLabel.text = textToType;
    }
}
