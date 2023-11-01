using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SetActive : MonoBehaviour
{
    
    public GameObject activeGameObject;

    public void ActivateObject()
    {
        if (activeGameObject.activeSelf != true)
        {
            activeGameObject.SetActive(true);
        }
        else
        {
            activeGameObject.SetActive(false);
        }
    }

}
