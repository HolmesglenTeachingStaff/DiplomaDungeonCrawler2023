using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_Shader : MonoBehaviour
{
    public Material mat;
    [ColorUsage(true,true)]
    public Color newColor;

    void Start()
    {
        mat=GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            mat.SetColor("_EmissionColor",newColor);
        }
    }
}
