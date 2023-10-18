using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingBean : MonoBehaviour
{
    Rigidbody rb;

    public LineRenderer line;
    public Camera cam;

    //For mouse mechanics
    //a line to draw based on the mouse position, and a refernece to the camera to cast from
    RaycastHit mouseHit;
    Ray mouseRay;

    public bool isGrounded;

    public float distanceToGround;
    public float jumpHeight;

    public LayerMask mask;

    RaycastHit hit;
    Ray ray;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ray.direction = Vector3.down;


        cam = Camera.main;
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        line.SetPosition(0, transform.position);

        #region Movement
        ray.origin = transform.position;
        isGrounded = Physics.Raycast(ray, distanceToGround, mask);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(ray, out hit, distanceToGround, mask))
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    Destroy(hit.collider.gameObject);
                }
                if (isGrounded)
                {
                    rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                }
            }
        }
        #endregion

        if (Input.GetMouseButtonDown(0))
        {
            mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out mouseHit, Mathf.Infinity))
            {
                Debug.Log(mouseHit.collider.name);

                line.SetPosition(1, mouseHit.point);
            }
        }
    }
}
