using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VK_Shader_Interaction : MonoBehaviour
{
    public Material mat;
    public Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mat.SetColor("_EmissiveColor", newColor);
        }
    }
}
