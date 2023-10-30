using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_ShaderDamage : MonoBehaviour
{
    public Material mat1;
    [ColorUsage(true,true)]
    public Color newColor1;

    void Start()
    {
        mat1=GetComponent<MeshRenderer>().material;
    }

    public void Flash()
    {
      mat1.SetColor("_EmissionColorD",newColor1);
    }
}
