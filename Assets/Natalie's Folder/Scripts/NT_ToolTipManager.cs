using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NT_ToolTipManager : MonoBehaviour
{
    public static NT_ToolTipManager _instance;

    public TextMeshProUGUI textComponent;

    // Start is called before the first frame update

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAndShowToolTip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
   
}
