using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public LayerMask mask;
    public bool interactable = true;

    private BoxCollider[] colliders;
    private Camera cam;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private Vector3 lockPos;
    private Vector3 startPosition;

    private void Start()
    {
        colliders = GetComponents<BoxCollider>();
        cam = Camera.main;
        startPosition = transform.position;

        switch (lockAxis)
        {
            case LockAxis.X: lockPos = transform.position + Vector3.right * bonusAxis; break;
            case LockAxis.Y: lockPos = transform.position + Vector3.up * bonusAxis; break;
            case LockAxis.Z: lockPos = transform.position + Vector3.forward * bonusAxis; break;
        }
    }

    private void OnMouseDown()
    {
        if (!interactable) return;

        // Save start Pos
        startPosition = transform.position;
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
        if (!interactable) return;

        dragged = false;

        for (int i = 0; i < colliders.Length; i++) // Loop though all colliders
        {
            BoxCollider currentCollider = colliders[i];

            // Check for collision with other tools only
            Collider[] overlapingColliders = Physics.OverlapBox(currentCollider.bounds.center, currentCollider.bounds.size / 2, Quaternion.identity, mask);

            for(int j = 0; j < overlapingColliders.Length; j++) // If colliding with another tool, return
            {
                if (overlapingColliders[j].TryGetComponent(out Tool tool) && tool != this)
                {
                    transform.position = startPosition;
                    return;
                }
            }
        }

        Toolbox.instance.CheckComplete();
    }
}
