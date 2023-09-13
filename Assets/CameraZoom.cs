using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraZoom : MonoBehaviour
{

    [Header("Zoom Settings")]
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float zoomSpeed = 2f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 2f;

    private CinemachineVirtualCamera virtualCamera;
    private float initialOrthographicSize;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    private void Start()
    {
        // Get the Cinemachine VirtualCamera component attached to the GameObject
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Store the initial orthographic size of the camera
        initialOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        // Zooming with the scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newOrthographicSize = Mathf.Clamp(
            virtualCamera.m_Lens.OrthographicSize - scrollInput * zoomSpeed,
            minZoom,
            maxZoom
        );
        virtualCamera.m_Lens.OrthographicSize = newOrthographicSize;

        // Rotating with middle mouse click
        if (Input.GetMouseButtonDown(2))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
        }

        if (isRotating)
        {
                 Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            float rotationAmount = deltaMousePosition.x * rotationSpeed;
            transform.Rotate(Vector3.up, rotationAmount, Space.World);
            lastMousePosition = Input.mousePosition;
        }
    }

    public void ResetZoom()
    {
        // Reset the orthographic size to its initial value
        virtualCamera.m_Lens.OrthographicSize = initialOrthographicSize;
    }
}