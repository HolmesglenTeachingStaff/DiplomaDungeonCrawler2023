using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NT_DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    public TextMeshProUGUI npcName;

    private void Start()
    {
        GetComponent<NT_TypeWriterEffect>().Run("This is a test", textLabel);
    }
}
