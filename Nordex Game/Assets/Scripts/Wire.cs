using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public LayerMask mask;
    public int index;
    public int connectedIndex;
    public bool interactable = true;

    private BoxCollider coreCollider;
    private Camera cam;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private Vector3 lockPos;
    private Vector3 startPosition;

    private void Start()
    {
        cam = Camera.main;
        coreCollider = GetComponent<BoxCollider>();
        startPosition = transform.position;

        switch (lockAxis)
        {
            case LockAxis.X: lockPos = transform.position + Vector3.right * bonusAxis; break;
            case LockAxis.Y: lockPos = transform.position + Vector3.up * bonusAxis; break;
            case LockAxis.Z: lockPos = transform.position + Vector3.forward * bonusAxis; break;
        }
    }

    private void OnMouseDrag()
    {
        if (!interactable) return;

        // Convert mouse position to a world point
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            // Calculate movement towards the mouse position
            switch (lockAxis)
            {
                case LockAxis.X: targetPosition = new Vector3(lockPos.x, hit.point.y, hit.point.z); break;
                case LockAxis.Y: targetPosition = new Vector3(hit.point.x, lockPos.y, hit.point.z); break;
                case LockAxis.Z: targetPosition = new Vector3(hit.point.x, hit.point.y, lockPos.z); break;
            }

            // Move the object towards the mouse position
            transform.position = targetPosition;
            dragged = true;
        }
    }

    private void OnMouseUp()
    {
        dragged = false;

        Collider[] colliders = Physics.OverlapBox(transform.position, coreCollider.size, Quaternion.identity, mask);

        if (colliders.Length == 0)
        {
            transform.position = startPosition;
            if (connectedIndex > -1) // Properly disconnect from connected wire socket
            {
                PanelWires.instance.placements[connectedIndex].GetComponent<BoxCollider>().enabled = true;
                connectedIndex = -1;
            }
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out PlacementBox box))
            {
                // Snap
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                // If you disconnect and reconnect with different shape - enable the last connected shape's collider
                if (connectedIndex >= 0 && connectedIndex != box.index) PanelWires.instance.placements[connectedIndex].GetComponent<BoxCollider>().enabled = true;
                transform.position = box.transform.position;
                connectedIndex = box.index;
                PanelWires.instance.placements[connectedIndex].GetComponent<BoxCollider>().enabled = false;
                PanelWires.instance.CheckComplete();
                placed = true;
                return;
            }
            else
            {
                transform.position = startPosition;
                if (connectedIndex > -1)
                {
                    PanelWires.instance.placements[connectedIndex].GetComponent<BoxCollider>().enabled = true;
                    connectedIndex = -1;
                }
            }
        }
    }
}
