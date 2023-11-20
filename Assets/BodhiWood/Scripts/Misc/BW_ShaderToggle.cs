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

    public Material flashMaterial;

    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    public void DamageFlashStart()
    {
        meshRenderer.material.color = flashMaterial.color;
        Invoke("DamageFlashStop", flashTime);
    }
    void DamageFlashStop()
    {
        meshRenderer.material.color = originalColor;
    }
}
