using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIcon : MonoBehaviour
{
    Camera cam;
    [SerializeField] MousePosition mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mousePosition.GetPosition(cam) + Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
