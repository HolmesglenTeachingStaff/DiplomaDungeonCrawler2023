using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is for toggling on a shader temporarily, 
/// then reset to default material
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

    //Change the material colour to flashMaterial, for duration of flashTime.
    public void DamageFlashStart()
    {
        meshRenderer.material.color = flashMaterial.color;
        Invoke("DamageFlashStop", flashTime);
    }
    //Change back to original colour
    void DamageFlashStop()
    {
        meshRenderer.material.color = originalColor;
    }
}
