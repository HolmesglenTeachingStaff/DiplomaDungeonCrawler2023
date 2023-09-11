using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternString : MonoBehaviour
{
    public Transform[] ropEnds;
    public LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, ropEnds[0].position);
        line.SetPosition(1, ropEnds[1].position);
    }
}
