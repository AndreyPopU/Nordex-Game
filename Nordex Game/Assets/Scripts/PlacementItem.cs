using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementItem : MonoBehaviour
{
    public int index;
    public bool interactable = true;

    private float cameraZ;
    private Camera cam;
    private Rigidbody rb;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        cameraZ = cam.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseDrag()
    {
        if (!interactable) return;

        // Convert mouse position to a world point
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate movement towards the mouse position
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 movementDirection = (targetPosition - transform.position).normalized;

            // Move the object towards the mouse position
            transform.position = targetPosition;
        }
    }
}
