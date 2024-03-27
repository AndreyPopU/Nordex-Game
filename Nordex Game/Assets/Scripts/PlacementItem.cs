using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PlacementItem : MonoBehaviour
{
    public int index;
    public bool interactable = true;

    private BoxCollider coreCollider;
    private Camera cam;
    public bool dragged;
    public bool placed;
    float yPos;

    private void Start()
    {
        cam = Camera.main;
        coreCollider = GetComponent<BoxCollider>();
        yPos = transform.position.y + .1f;
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
            Vector3 targetPosition = new Vector3(hit.point.x, yPos, hit.point.z);

            // Move the object towards the mouse position
            transform.position = targetPosition;
            dragged = true;
        }
    }

    private void OnMouseUp()
    {
        dragged = false;

        Collider[] colliders = Physics.OverlapBox(transform.position, coreCollider.size, Quaternion.identity);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out PlacementBox box) && box.index == index)
            {
                // Snap
                interactable = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = box.transform.position;
                colliders[i].GetComponent<PlacementBox>().full = true;
                placed = true;
                Toolbox.instance.CheckComplete();
            }
            
        }
    }
}
