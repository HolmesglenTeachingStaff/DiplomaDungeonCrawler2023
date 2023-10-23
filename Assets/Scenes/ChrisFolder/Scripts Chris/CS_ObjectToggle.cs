using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ObjectToggle : MonoBehaviour
{

    public GameObject objectToToggle;



    public void ToggleObject(int fakeBool) //this is to convert an int into a bool because the animator tab doesn't allow bools internally
    {
        bool realBool = Mathf.Approximately(fakeBool, 1);

        objectToToggle.SetActive(realBool);
    }


}
