using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Cog : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public LayerMask mask;
    public int index;
    public bool interactable = true;
    public PlacementBox socket;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private BoxCollider coreCollider;
    private Camera cam;

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

        // Remove from parent socket
        if (transform.parent != null) transform.SetParent(null);

        // Stop spinning
        transform.rotation = Quaternion.identity;

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
            ResetPos();
            return;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            print(colliders[i].gameObject.name);

            if (colliders[i].TryGetComponent(out PlacementBox box))
            {
                if (box.index != index)
                {
                    ResetPos();
                    continue;
                }
                // Snap
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = box.transform.position;
                colliders[i].GetComponent<BoxCollider>().enabled = false;
                placed = true;
                socket = box;
                box.full = true;
                transform.SetParent(box.transform);
                Clockwork.instance.CheckComplete();
                return;
            }
            else ResetPos();
        }
    }

    public void ResetPos()
    {
        if (transform.parent != null) transform.SetParent(null);
        transform.position = startPosition;
        if (socket != null)
        {
            socket.GetComponent<BoxCollider>().enabled = true;
            socket.full = false;
        }
        transform.rotation = Quaternion.identity;
        placed = false;
        socket = null;
    }
}
