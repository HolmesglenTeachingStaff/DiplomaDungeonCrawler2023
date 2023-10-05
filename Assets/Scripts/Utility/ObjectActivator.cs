using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] bool isCollider;

    public void ActivateObject(string fakeBool)
    {
        if(fakeBool.ToLowerInvariant() == "true")
        {
            if (isCollider) objectToActivate.GetComponent<Collider>().enabled = true;
            else objectToActivate.SetActive(true);
        }
        if (fakeBool.ToLowerInvariant() == "false")
        {
            if (isCollider) objectToActivate.GetComponent<Collider>().enabled = false;
            else objectToActivate.SetActive(false);
        }
    }
}
