using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ToolTip : MonoBehaviour
{
    public Transform player;
    public float playerRange;
    public Color playerRangeColor;
    public string message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRange(playerRange))
        {
            CS_ToolTipManager._instance.SetAndShowToolTip(message);
        }
        else
        {
            CS_ToolTipManager._instance.HideToolTip();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = playerRangeColor;
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }

    bool IsInRange(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
