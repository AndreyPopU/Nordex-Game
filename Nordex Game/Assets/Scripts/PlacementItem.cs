using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PlacementItem : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public int index;
    public bool interactable = true;

    private BoxCollider coreCollider;
    private Camera cam;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private Vector3 lockPos;

    private void Start()
    {
        cam = Camera.main;
        coreCollider = GetComponent<BoxCollider>();

        switch (lockAxis)
        {
            case LockAxis.X: lockPos = transform.position + Vector3.right * bonusAxis; break;
            case LockAxis.Y: lockPos = transform.position + Vector3.up * bonusAxis; break;
            case LockAxis.Z: lockPos = transform.position + Vector3.forward * bonusAxis; break;
        }
    }

    private void OnMouseDrag()
    {
        //if (!interactable) return;

        if (Input.GetMouseButton(0))
        {
            // Convert mouse position to a world point
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                print(hit.collider.gameObject.name);

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
    }

    private void OnMouseUp()
    {
        dragged = false;

        Collider[] colliders = Physics.OverlapBox(transform.position, coreCollider.size, Quaternion.identity, 7);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out PlacementBox box) && box.index == index)
            {
                print(box.index + " " + index);
                // Snap
                interactable = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = box.transform.position;
                colliders[i].GetComponent<PlacementBox>().full = true;
                placed = true;
                //Toolbox.instance.CheckComplete();
            }
            
        }
    }
}
