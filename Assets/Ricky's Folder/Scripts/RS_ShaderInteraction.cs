using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RS_ShaderInteraction : MonoBehaviour
{

    public Material mat;

   [ColorUsage(true, true)]
    public Color newColor;



    // Start is called before the first frame update
    void Start()
    {

       mat = GetComponent<MeshRenderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            mat.SetColor("_Color", newColor);

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            //mat.SetFloat(_"AlphaValue", 1);
        }
        
    }
}
