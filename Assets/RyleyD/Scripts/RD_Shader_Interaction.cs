using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RD_Shader_Interaction : MonoBehaviour
{
    public Material mat;

    //make the color HDR accessible
    [ColorUsage(true, true)]
    public Color newColor;
    private Color originColor;

    private bool isColorChanging = false, isAlphaChanging = false;


    // Start is called before the first frame update
    void Start()
    {
        //find material instance based on meshRenderer;
        mat = GetComponent<MeshRenderer>().material;
        //save the original color
        originColor = mat.GetColor("_EmissiveColor");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("ColorLerp");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine("AlphaLerp");
        }
    }
    public IEnumerator AlphaLerp()
    {
        if (isAlphaChanging) yield return null;
        isAlphaChanging = true;

        float timeStep = 0;
        float duration = 2f;
        float alphaFloat;

        while (timeStep <= duration)
        {
            alphaFloat = Mathf.Lerp(0, 1, timeStep / duration);
            mat.SetFloat("_AlphaValue", alphaFloat);
            yield return new WaitForEndOfFrame();
            timeStep += Time.deltaTime;
        }

        isAlphaChanging = false;
        yield return null;
    }
    public IEnumerator ColorLerp()
    {
        if (isColorChanging) yield return null;
        isColorChanging = true;

        float timeStep = 0;
        float duration = 2f;
        Color lerpedColor;

        while (timeStep <= duration)
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
