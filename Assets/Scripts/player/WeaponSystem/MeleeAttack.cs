using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAttack : MonoBehaviour
{
    public StatSystem.DamageType damageType;

    public float minDamage;
    public float maxDamage;

    public GameObject particleEffect;
    public GameObject weaponCollider;

    public PlayerIKHandling playerIKHandler;
    public PlayerMovment playerMovement;

    public int noOfClicks = 0;
    private float lastClickedTime;
    public float nextFireTime = 0f;
    public float maxComboDelay;

    Animator anim;

    //VFX components
    public PlayVFX VFXScript;
    public VisualEffect[] VFXObjects;
    public VisualEffectAsset[] VFXassets;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        
        if(Time.time - lastClickedTime > maxComboDelay || Input.GetButton("Jump"))
        {
            noOfClicks = 0;
            anim.SetInteger("HitNum", noOfClicks);
            //playerIKHandler.ikActive = false;
        }
       
        if (Input.GetButtonDown("Fire1") && playerMovement.currentState != PlayerMovment.state.Dashing )
        {
            OnClick();
        }
        

    }
    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        anim.SetInteger("HitNum", noOfClicks);
        
    }
    public void UpdateVFX()
    {
        int index = (int)damageType;
        for (int i = 0; i < VFXObjects.Length; i++)
        {
            VFXObjects[i].visualEffectAsset = VFXassets[index];
        }
    }

}
