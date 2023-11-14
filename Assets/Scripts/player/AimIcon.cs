using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIcon : MonoBehaviour
{
    Camera cam;
    GameObject player;
    //[SerializeField] MousePosition mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = MousePosition.mousePosition + Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
