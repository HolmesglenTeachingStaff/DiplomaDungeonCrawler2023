using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JP_ShaderDamge2 : MonoBehaviour
{
    public List<Material> mat1 = new List<Material>();
    [ColorUsage(true,true)]
    public Color newColor1;

    void Start()
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(SkinnedMeshRenderer rend in renderers)
        {
            mat1.Add(rend.material);
        }
        //mat1=GetComponentsInChildren<SkinnedMeshRenderer>().materials;
    }

    public void FlashActive()
    {
      StartCoroutine(Flash());
    }

    public IEnumerator Flash()
    {
      //mat1.SetColor("_EmissionColorD",newColor1);
      foreach(Material mat in mat1)
      {
        mat.SetFloat("_Float",0.5f);
      }
      yield return new WaitForSeconds(0.10f);
      foreach(Material mat in mat1)
      {
        mat.SetFloat("_Float",1f);
      }
      yield return new WaitForSeconds(0.25f);
      foreach(Material mat in mat1)
      {
        mat.SetFloat("_Float",0f);
      }
      yield return null;
    }
}
