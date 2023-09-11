using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinBob : MonoBehaviour
{


    public float bobDistance;
    public float bobSpeed;
    private Vector3 startPos, direction;

    public enum Axis { x, y, z };
    public Axis axis;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        switch (axis)
        {
            case Axis.x: direction = Vector3.right; break;
            case Axis.y: direction = Vector3.up; break;
            case Axis.z: direction = Vector3.forward; break;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float posModifier = Mathf.Sin(Time.time * bobSpeed) * bobDistance;
        transform.position = startPos + direction * posModifier;
    }
}
