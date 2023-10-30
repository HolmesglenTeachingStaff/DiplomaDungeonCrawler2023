using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJ_ShaderInteraction : MonoBehaviour
{
    public Material mat;

    //make the color hdr accessible
    [ColorUsage(true, true)]
    public Color newColor;
    
    public Color originColor;
    
    private bool isColorChanging = false, isAlphaChanging = false;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        originColor = mat.GetColor("_EmissiveColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ColorLerp());
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(AlphaLerp());
        }
    }

    public IEnumerator AlphaLerp()
    {
        Debug.Log("Coroutine Started");
        if (isAlphaChanging) yield return null;
        Debug.Log("not stopped");
        isAlphaChanging = true;

        float timeStep = 0;
        float duration = 2f;
        float alphaFloat = 0;

        while (timeStep <= duration)
        {
            Debug.Log(alphaFloat);
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
        Debug.Log("Coroutine started");
        if (isColorChanging) yield return null;
        Debug.Log("not stopped");
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
