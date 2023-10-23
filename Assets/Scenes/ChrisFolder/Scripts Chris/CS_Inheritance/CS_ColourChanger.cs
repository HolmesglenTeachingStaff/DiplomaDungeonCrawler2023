using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ColourChanger : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GetComponent<Renderer>().material.color = Color.magenta;
    }
}