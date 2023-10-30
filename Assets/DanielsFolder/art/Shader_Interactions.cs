using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_Interactions : MonoBehaviour
{
    public Material mat;

    [ColorUsage(true,true)]
    public Color defaultColour, newColour;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        defaultColour = mat.GetColor("_EmissiveColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeAlpha();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeColour();
        }
    }

    public void ChangeColour()
    {
        mat.SetColor("_EmissiveColor", newColour);
    }
    public void ChangeAlpha()
    {
        mat.SetFloat("_AlphaValue", 1);
    }
}
