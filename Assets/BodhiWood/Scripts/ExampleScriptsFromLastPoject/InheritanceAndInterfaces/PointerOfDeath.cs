using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOfDeath : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //try and damage a base stat
                BaseStats stats = hit.collider.GetComponent<BaseStats>();

                if (stats == null)
                {
                    Debug.Log("Can't find a stat block");
                }
                else
                {
                    stats.DoDamage(5);
                }

                IInteractable item = hit.collider.GetComponent<IInteractable>();
                if (item == null)
                {
                    return;
                }
                else
                {
                    item.Interact();
                }
            }
        }
    }
}
