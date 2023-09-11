using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousRotation : MonoBehaviour
{
    public float turnSpeed;

    private Vector3 direction;

    public enum Axis { x, y, z };
    public Axis axis;
    // Start is called before the first frame update
    void Start()
    {
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
        transform.Rotate(direction, turnSpeed);
    }
}
