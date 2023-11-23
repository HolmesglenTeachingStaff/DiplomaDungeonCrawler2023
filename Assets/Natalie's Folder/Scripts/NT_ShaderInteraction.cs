using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NT_ShaderInteraction : MonoBehaviour
{
    public Material mat;

    //Allows us to see alpha and intesity levels, make color HDR accessible 
    [ColorUsage(true, true)]
    public Color newColor;
    private Color originColor;
    private bool isColorChanging = false, isAlphaChanging = false;
    // Start is called before the first frame update
    void Start()
    {
        //Find material instance based on meshrenderer
        mat = GetComponent<MeshRenderer>().material;
        originColor = mat.GetColor("EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine("ColorLerp");
        }

        if (Input.GetKeyDown(KeyCode.G))
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
