using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_DeathShader : MonoBehaviour
{
    public Material mat;
    DB_Nekomata nekomataFSM;
    private bool isAlphaChanging;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (nekomataFSM.currentState == DB_Nekomata.States.DIE)
        {
            StartCoroutine("DeathAlpha");
        }
        else
        {
            StopCoroutine("DeathAlpha");
        }
    }
    IEnumerator DeathAlpha()
    {
        if (isAlphaChanging) yield return null;
        isAlphaChanging = true;

        float timeStep = 0f;
        float duration = 1f;
        float alphaFloat = 0f;

        while(timeStep <= duration)
        {
            alphaFloat = Mathf.Lerp(0, 1, timeStep / duration);
            mat.SetFloat("_AlphaValue", alphaFloat);
            yield return new WaitForEndOfFrame();
            timeStep += Time.deltaTime;
        }
        isAlphaChanging = false;
        yield return null;
    }
}

