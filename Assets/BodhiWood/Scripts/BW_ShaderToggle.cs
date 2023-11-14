using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is for toggling on a shader temporarily, 
/// then reset to previous/default material
/// </summary>

public class BW_ShaderToggle : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    Color originalColor;
    float flashTime = 0.15f;

    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        originalColor = meshRenderer.material.color;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DamageReceivedStart();
        }
    }

    public void DamageReceivedStart()
    {
        meshRenderer.material.color = Color.red;
        Invoke("DamageReceivedStop", flashTime);
    }
    public void DamageReceivedStop()
    {
        meshRenderer.material.color = originalColor;
    }
}
