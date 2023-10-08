using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public StatSystem.DamageType currentDamageType;
    private int weaponIndex = 0;
    private float scrollBuffer = 0.25f;

    //script Assets
    private MeleeAttack meleeScript;
    private DamageOnTouch meleeObject;
    public LanternAttack lanternScript;
    private Renderer lanternRenderer;
    private Renderer bladeRenderer;
    public GameObject[] weaponImages;
    public GameObject lantern;
    public GameObject blade;
    public Light pointLight;
    public float glowPower;
    public Color Physical;
    public Color Fire;
    public Color Water;
    public Color Wind;
    public Color Earth;
    public Color Light;
    public Color Dark;

    // Start is called before the first frame update
    void Start()
    {
        meleeScript = GetComponent<MeleeAttack>();
        meleeObject = meleeScript.weaponCollider.GetComponent<DamageOnTouch>();
        
        currentDamageType = (StatSystem.DamageType)weaponIndex;
        lanternRenderer = lantern.GetComponent<Renderer>();
        bladeRenderer = blade.GetComponent<Renderer>();
        UpdateWeapon(0);
        UpdateWeaponImages();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y > (0f + scrollBuffer))
        {
            weaponIndex += 1;
            if (weaponIndex >= 5) weaponIndex = 0;
            UpdateWeapon(weaponIndex);
            //lanternRenderer.material.SetColor("_AlbedoColor", Physical *glowPower);
        }
        else if(Input.mouseScrollDelta.y < (0f - scrollBuffer))
        {
            weaponIndex -= 1;
            if (weaponIndex <= -1) weaponIndex = 4;
            UpdateWeapon(weaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponIndex = 0;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Physical* glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Physical* glowPower);
            pointLight.color = Physical;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponIndex = 1;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Fire* glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Fire* glowPower);
            pointLight.color = Fire;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponIndex = 2;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Water * glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Water* glowPower);
            pointLight.color = Water;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponIndex = 3;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Wind * glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Wind* glowPower);
            pointLight.color = Wind;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponIndex = 4;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Earth *glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Earth* glowPower);
            pointLight.color = Earth;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weaponIndex = 5;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Light * glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Light* glowPower);
             pointLight.color = Light;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            weaponIndex = 6;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_AlbedoColor", Dark * glowPower);
            bladeRenderer.material.SetColor("_AlbedoColor", Dark* glowPower);
            pointLight.color = Dark;
        }
    }
    void UpdateWeapon(int weaponIndex)
    {
        currentDamageType = (StatSystem.DamageType)weaponIndex;
        UpdateWeaponImages();
        meleeScript.damageType = currentDamageType;
        meleeObject.damageType = currentDamageType;
        meleeScript.UpdateVFX();
        lanternScript.damageType = currentDamageType;
    }
    void UpdateWeaponImages()
    {
        if (weaponImages[0] == null) return;
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i == weaponIndex) weaponImages[i].SetActive(true);
            else weaponImages[i].SetActive(false);
        }
    } 
}
