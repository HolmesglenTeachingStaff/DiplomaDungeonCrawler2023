using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    #region Variables
    //Instantiation Variables
    public Camera cam;
    public LayerMask mask;

    public GameObject objectToBeSpawned;
    Vector3 spawnLocation = Vector3.zero;
    Quaternion spawnRotation;

    RaycastHit hit;
    Ray ray;

    //Object Preview Variables
    public GameObject previewObject;
    #endregion

    void Start()
    {
        cam = Camera.main;

        previewObject.SetActive(false);
    }

    void Update()
    {
        ObjectCreationAndDeletion();
        ObjectPreview();
    }

    void ObjectCreationAndDeletion()
    {
        #region Spawn Objects On Location Clicked
        //Set on click events with Input.GetMouseButtonDown
        if (Input.GetMouseButtonUp(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            spawnRotation = Quaternion.LookRotation(spawnLocation);

            previewObject.SetActive(false);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                //Instantiate a prefabbed object in the specified location
                Instantiate(objectToBeSpawned, hit.point + Vector3.up, spawnRotation);
            }
        }
        #endregion

        #region Delete Spawned Objects
        //Delete spawned Objects
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider.gameObject.tag == "PlacedObject")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        #endregion
    }

    void ObjectPreview()
    {
        #region Display Preview Object On Mouse Position
        if (Input.GetMouseButton(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                previewObject.SetActive(true);
                previewObject.transform.position = hit.point + Vector3.up;
            }
        }
        #endregion
    }
}
