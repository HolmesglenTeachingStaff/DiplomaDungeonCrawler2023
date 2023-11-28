using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IB_Shader_Interaction : MonoBehaviour
{
    public Material mat;

    //make the color hdr accessible
    [ColorUsage(true, true)]
    public Color newColor;

    private Color originColor;

    private bool isColorChanging = false, isAlphaChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        //find material instance based on meshRenderer;
        mat = GetComponent<MeshRenderer>().material;
        //save the original
        originColor = mat.GetColor("_EmissiveColor");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            mat.SetColor("_EmissiveHDRColor", newColor);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ColorLerp());
        }
    }
    public IEnumerator ColorLerp()
    {
        Debug.Log("coroutine started");
        if (isAlphaChanging) yield return null;
        Debug.Log("not stopped");
        isAlphaChanging = true;

        float timeStep = 0;
        float duration = 2f;
        Color lerpedColor;

        while(timeStep <= duration)
        {
            lerpedColor = Color.Lerp(originColor, newColor, timeStep / duration);
            mat.SetColor("_EmissiveColor", lerpedColor);
            yield return new WaitForEndOfFrame();
            timeStep += Time.deltaTime;
        }

        isAlphaChanging = false;
        yield return null;
    }
}
