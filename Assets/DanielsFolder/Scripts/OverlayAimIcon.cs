using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayAimIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(MousePosition.mousePosition + Vector3.up);
        GetComponent<RectTransform>().position = screenPosition;
    }
}
