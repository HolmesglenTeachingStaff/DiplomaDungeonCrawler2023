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

    public void FlashActive()
    {
      StartCoroutine(Flash());
    }

    public IEnumerator Flash()
    {
      //mat1.SetColor("_EmissionColorD",newColor1);
      mat1.SetFloat("_Float",0.5f);
      yield return new WaitForSeconds(0.10f);
      mat1.SetFloat("_Float",1f);
      yield return new WaitForSeconds(0.25f);
      mat1.SetFloat("_Float",0f);
      yield return null;
    }
}
