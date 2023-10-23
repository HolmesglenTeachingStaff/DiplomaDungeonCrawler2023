using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_JumpingBean : MonoBehaviour
{

    Rigidbody beanRb;

    //BELOW 4 LINES ARE FOR MOUSE TO SCREEN RAYS
    public LineRenderer line;
    public Camera cam;

    RaycastHit mouseHit;
    Ray mouseRay;

    public bool grounded;
    public float distance;
    public float jumpHeight;


    RaycastHit hit;

    public LayerMask mask;




    // Start is called before the first frame update
    void Start()
    {
        beanRb = GetComponent<Rigidbody>();

        cam = Camera.main;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, distance, mask); //this was added as a check instead of adding this check below to not cause double jumping, this is so it can be added to multiple objects, mask is added to check the mask it's on, if on wrong mask it can't be grounded and can't jump
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*if(grounded /*Physics.Raycast(transform.position, Vector3.down, distance)*/ //) //this has the position we're starting at (transform.position), then the direction down for the raycast to check, then the distance variable we created
           /* {
                beanRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            }*/
            if(Physics.Raycast(transform.position, Vector3.down, out hit, distance, mask))
            {
                //Debugging point hit, distance, game object hits' name
                Debug.Log(hit.point);
                Debug.Log(hit.distance);
                Debug.Log(hit.collider.gameObject.name);

                if(hit.collider.gameObject.tag == "Enemy") //if we were to jump on the enemy, it will then destroy it after jumping off
                {
                    Destroy(hit.collider.gameObject);

                }

            }
            if(grounded)
            {
                beanRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            }

        }

        //for mouse ray inputs
        if (Input.GetMouseButtonDown(0))
        {
            mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity))
            {
                Debug.Log(mouseHit.collider.name);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, mouseHit.point);
            }
        }

    }
}
