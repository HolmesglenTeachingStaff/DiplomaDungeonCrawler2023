using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_Shader_Interactions : MonoBehaviour
{
    public Material mat;

    [ColorUsage(true, true)]
    public Color newColor;
    private Color originColor;
    private bool isColorChanging = false, isAlphaChanging = false;

    void Start()
    {
        //Find material instance based off of meshRenderer
        mat = GetComponent<MeshRenderer>().material;
        //Save the original color
        originColor = mat.GetColor("_EmissiveColor");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ColorLerp());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(AlphaLerp());
        }
    }

    public IEnumerator AlphaLerp()
    {
        if (isAlphaChanging == true) yield return null;
        isAlphaChanging = true;

        float timeStep = 0f;
        float duration = 2f;
        float alphaFloat = 0;

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
        if (isColorChanging == true) yield return null;
        isColorChanging = true;

        float timeStep = 0f;
        float duration = 2f;
        Color lerpedColor;

        while (timeStep <= duration)
        {
            lerpedColor = Color.Lerp(originColor, newColor, timeStep / duration);
            mat.SetColor("_EmissiveColor", lerpedColor);

            yield return new WaitForEndOfFrame();

            timeStep += Time.deltaTime;
        }

        isColorChanging = false;
        yield return null;
    }
}
