using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    //Camera cam;
    Ray ray;
    Plane plane;
    Vector3 position;
    Vector3 planePosition;

    // Start is called before the first frame update
    public void Start()
    {

        
        

    }

    public Vector3 GetPosition(Camera cam, Vector3 planePosition = default(Vector3))
    {

        plane = new Plane(Vector3.up, planePosition);

        ray = cam.ScreenPointToRay(Input.mousePosition);

        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            position = ray.GetPoint(enter);
        }
        return position;
    }
}
    
