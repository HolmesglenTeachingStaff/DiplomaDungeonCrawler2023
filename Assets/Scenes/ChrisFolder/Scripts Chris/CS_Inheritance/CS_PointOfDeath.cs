using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PointOfDeath : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //try and damage a base stat
                CS_BaseStats stats = hit.collider.GetComponent<CS_BaseStats>();

                if(stats == null)
                {
                    Debug.Log("Can't find a stat block");
                }
                else
                {
                    stats.DoDamage(5);
                }
            }
        }
       
    }
}