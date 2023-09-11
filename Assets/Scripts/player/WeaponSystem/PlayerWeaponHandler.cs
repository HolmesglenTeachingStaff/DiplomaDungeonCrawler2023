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
    private BowAttack bowScript;
    private Renderer lanternRenderer;
    public GameObject[] weaponImages;
    public GameObject lantern;
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
        bowScript = GetComponent<BowAttack>();
        currentDamageType = (StatSystem.DamageType)weaponIndex;
        lanternRenderer = lantern.GetComponent<Renderer>();
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
            //lanternRenderer.material.SetColor("_EmissionColor", Physical *10f);
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
            lanternRenderer.material.SetColor("_EmissionColor", Physical*10);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponIndex = 1;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Fire*10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponIndex = 2;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Water *10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponIndex = 3;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Wind *10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponIndex = 4;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Earth *10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weaponIndex = 5;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Light * 10f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            weaponIndex = 6;
            UpdateWeapon(weaponIndex);
            lanternRenderer.material.SetColor("_EmissionColor", Dark * 10f);
        }
    }
    void UpdateWeapon(int weaponIndex)
    {
        currentDamageType = (StatSystem.DamageType)weaponIndex;
        UpdateWeaponImages();
        meleeScript.damageType = currentDamageType;
        meleeObject.damageType = currentDamageType;
        meleeScript.UpdateVFX();
        bowScript.selectedQuiver = weaponIndex;
        bowScript.UpdateArrowUI();
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
