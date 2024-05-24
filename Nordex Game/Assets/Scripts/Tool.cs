using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public LayerMask mask;
    public bool interactable = true;

    private Collider[] colliders;
    private Camera cam;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private Vector3 lockPos;
    private Vector3 startPosition;
    private GameObject overlay;
    private GameObject GFX;

    Vector3 dir;
    float dist;

    private void Start()
    {
        colliders = GetComponents<Collider>();
        cam = Camera.main;
        startPosition = transform.position;

        switch (lockAxis)
        {
            case LockAxis.X: lockPos = transform.position + Vector3.right * bonusAxis; break;
            case LockAxis.Y: lockPos = transform.position + Vector3.up * bonusAxis; break;
            case LockAxis.Z: lockPos = transform.position + Vector3.forward * bonusAxis; break;
        }

        GFX = transform.GetChild(0).gameObject;
        overlay = transform.GetChild(1).gameObject;
    }

    private void OnMouseDown()
    {
        if (!interactable) return;

        // Save start Pos
        startPosition = transform.position;

        // Enable Overlay
        //GFX.SetActive(false);
        overlay.SetActive(true);
    }

    float rotationSpeed = 10f;
    private void OnMouseDrag()
    {
        // Mouse Rotation
        float XaxisRotation = Input.mouseScrollDelta.y * rotationSpeed;
        transform.Rotate(Vector3.forward, XaxisRotation);
          
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
            
             
            for (int i = 0; i < colliders.Length; i++) // Loop though all colliders
            {
                Collider currentCollider = colliders[i];

                // Check for collision with other tools only
                Collider[] overlapingColliders = Physics.OverlapBox(currentCollider.bounds.center, currentCollider.bounds.size / 2, Quaternion.identity, mask);

                for (int j = 0; j < overlapingColliders.Length; j++) // If colliding with another tool, return
                {
                    // Use Physics.ComputePenetration to check if the actual mesh colliders are colliding
                    if (overlapingColliders[j].TryGetComponent(out Tool tool) && tool != this &&
                        Physics.ComputePenetration(currentCollider, currentCollider.transform.position, currentCollider.transform.rotation,
                        overlapingColliders[j], overlapingColliders[j].transform.position, overlapingColliders[j].transform.rotation, out dir, out dist))
                    {
                        // Red Overlay
                        foreach (Material mat in overlay.GetComponent<MeshRenderer>().materials)
                            mat.color = new Color(1, 0, 0, .3f); // Red

                        return;
                    }
                    else if (overlapingColliders[j].gameObject.name == "OutsideCollider")
                    {
                        // Red Overlay
                        foreach (Material mat in overlay.GetComponent<MeshRenderer>().materials)
                            mat.color = new Color(1, 0, 0, .3f); // Red

                        return;
                    }
                }

                // Green Overlay
                foreach (Material mat in overlay.GetComponent<MeshRenderer>().materials)
                    mat.color = new Color(0, 1, 0, .3f); // Green
            }
        }
    }

    private void OnMouseUp()
    {
        if (!interactable) return;

        dragged = false;

        // Enable Overlay
        GFX.SetActive(true);
        overlay.SetActive(false);


        for (int i = 0; i < colliders.Length; i++) // Loop though all colliders
        {
            Collider currentCollider = colliders[i];

            // Check for collision with other tools only
            Collider[] overlapingColliders = Physics.OverlapBox(currentCollider.bounds.center, currentCollider.bounds.size / 2, Quaternion.identity, mask);

            for(int j = 0; j < overlapingColliders.Length; j++) // If colliding with another tool, return
            {
                // Use Physics.ComputePenetration to check if the actual mesh colliders are colliding
                if (overlapingColliders[j].TryGetComponent(out Tool tool) && tool != this &&
                    Physics.ComputePenetration(currentCollider, currentCollider.transform.position, currentCollider.transform.rotation,
                    overlapingColliders[j], overlapingColliders[j].transform.position, overlapingColliders[j].transform.rotation, out dir, out dist))
                {
                    transform.position = startPosition;
                    return;
                }
                else if (overlapingColliders[j].gameObject.name == "OutsideCollider")
                {
                    transform.position = startPosition;
                    return;
                }
            }
        }

        Toolbox.instance.CheckComplete();
    }
}
