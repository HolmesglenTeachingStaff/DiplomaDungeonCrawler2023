using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NT_LowHealthShader : MonoBehaviour
{
    public Material mat;
    NT_OniStateMachine oniStateMachine;
    private bool isAlphaChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(oniStateMachine.currentState == NT_OniStateMachine.States.RETREAT)
        {
            StartCoroutine("AlphaLerp");
        }
        else
        {
            StopCoroutine("AlphaLerp");
        }
    }

    public IEnumerator AlphaLerp()
    {
        if (isAlphaChanging) yield return null;
        isAlphaChanging = true;

        float timeStep = 0;
        float duration = 1f;
        float alphaFloat = 0;

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
