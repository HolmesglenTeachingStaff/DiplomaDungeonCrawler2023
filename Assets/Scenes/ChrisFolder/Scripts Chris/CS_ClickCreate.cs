using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ClickCreate : MonoBehaviour
{
    //get gameobject to instantiate
    public GameObject summoned;

    //position to summon
    Vector3 startPosition;

    //create mouse to screen rays linerenderer and cam
    public LineRenderer line;
    public Camera cam;

    //create mouse to screen ray raycasting
    RaycastHit mouseHit;
    Ray mouseRay;

    //create position raycast has hit
    RaycastHit hit;

    //rigidbody for instantiated object? (not sure if needed)
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        
        //gameobject to spawn reffrence position
        startPosition = summoned.transform.position;
        
        //camera refference
        cam = Camera.main;

        //line renderer refference
        line = GetComponent<LineRenderer>();
        
    }

    // call mouse button down to show line/box, while mouse button up (release) drops the object
    void Update()
    {
        //if statement for mouse button down (show the object before summoning here)
        if (Input.GetMouseButtonDown(0))
        {
            ShowBox();
        }
        //if statement for lifting mouse button back up (summon the object here)
        if (Input.GetMouseButtonUp(0))
        {
            mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity))
            {
                Debug.Log(mouseHit.collider.name);
                line.SetPosition(0, transform.position);

                //create instantiate position
                line.SetPosition(1, mouseHit.point);
                


                Instantiate(summoned, mouseHit.point, Quaternion.identity);
            }
        }

    }

    void ShowBox()
    {
        mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity))
            {
                Debug.Log(mouseHit.collider.name);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, mouseHit.point);

                //create seethrough block to project here

                
            }

    }

}
