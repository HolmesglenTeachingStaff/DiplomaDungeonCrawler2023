using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKToggle : MonoBehaviour
{
    public PlayerIKHandling IKHandler;

    public void ToggleIK(int isActive)
    {
        IKHandler.ikActive = (isActive == 1);
    }

}
