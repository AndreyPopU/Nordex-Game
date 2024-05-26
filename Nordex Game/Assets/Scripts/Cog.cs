using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Cog : MonoBehaviour
{
    public enum LockAxis { X, Y, Z }

    public LockAxis lockAxis;
    public LayerMask mask;
    public int index;
    public bool interactable = true;
    public CogSocket socket;
    public bool dragged;
    public bool placed;
    public float bonusAxis;

    private BoxCollider coreCollider;
    private Camera cam;

    private Vector3 lockPos;
    private Vector3 startPosition;

    private void Start()
    {
        // Setup
        cam = Camera.main;
        coreCollider = GetComponent<BoxCollider>();
        startPosition = transform.position;

        // Select axis to lock to when dragged by mouse
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

        // Remove from parent socket
        transform.SetParent(Clockwork.instance.transform);

        // If not attached
        if (socket != null)
        {
            // Enable socket logic
            socket.GetComponent<BoxCollider>().enabled = true;
            socket.full = false;
        }

        // Stop spinning
        transform.rotation = Quaternion.identity;
    }

    private void OnMouseDrag()
    {
        if (!interactable) return;

        // Convert mouse position to a world point
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Send Raycast from screen to world position
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

        // Check all sockets in range of Cog
        Collider[] colliders = Physics.OverlapBox(transform.position, coreCollider.size, Quaternion.identity, mask);

        // If no sockets were in range, reset the position
        if (colliders.Length == 0)
        {
            ResetPos();
            return;
        }

        // Find all Possible sockets
        List<CogSocket> possibleSockets = new List<CogSocket>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out CogSocket socket))
            {
                // If socket isn't matching or is full
                if (socket.index != index || socket.full) continue;

                possibleSockets.Add(socket);
            }
        }

        // If no sockets, reset & return
        if (possibleSockets.Count <= 0)
        {
            ResetPos();
            return;
        }

        Vector3 targetPosition = Vector3.zero;

        // Convert mouse position to a world point
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;
        }

        // Find the closest socket
        CogSocket closestSocket = possibleSockets[0];
        float closestDistance = Vector3.Distance(targetPosition, closestSocket.transform.position);

        for (int f = 1; f < possibleSockets.Count; f++)
        {
            if (Vector3.Distance(targetPosition, possibleSockets[f].transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(targetPosition, closestSocket.transform.position);
                closestSocket = possibleSockets[f];
            }
        }

        if (socket != null) ResetSocket();

        // Snap to Socket
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = closestSocket.transform.position;
        closestSocket.GetComponent<BoxCollider>().enabled = false;
        placed = true;
        this.socket = closestSocket;
        closestSocket.full = true;
        transform.SetParent(closestSocket.transform);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        closestSocket.Run();
        Clockwork.instance.CheckComplete();
        return;
    }

    public void ResetPos() // Reset position and socket
    {
        if (transform.parent != null) transform.SetParent(null);
        transform.position = startPosition;
        ResetSocket();
    }

    public void ResetSocket()
    {
        if (socket != null)
        {
            // Enable socket again
            socket.GetComponent<BoxCollider>().enabled = true;
            socket.full = false;

            // If head is removed
            if (socket == socket.head)
            {
                socket.frontNeighbours[0].running = false;
                socket.frontNeighbours[0].CheckFront();
                return;
            }

            // Update chain
            socket.running = false;
            socket.Run();

            // Check if any of the neighbours were rusted
            foreach (CogSocket socket in socket.backNeighbours)
            {
                if (socket.rusted)
                {
                    socket.head.running = true;
                    socket.head.CheckFront();
                    Clockwork.instance.PlayRandomClip();
                }
            }

            foreach (CogSocket socket in socket.frontNeighbours)
            {
                if (socket.rusted)
                {
                    socket.head.running = true;
                    socket.head.CheckFront();
                    Clockwork.instance.PlayRandomClip();
                }
            }

            // Check if the whole chain leads to a rusted chain
            CogSocket currentSocket = socket.head;
            while (currentSocket != null && currentSocket.full)
            {
                if (currentSocket.frontNeighbours.Length <= 0) return;

                currentSocket = currentSocket.frontNeighbours[0];

                if (currentSocket.rusted) return;
            }

            socket.head.running = true;
            socket.head.CheckFront();
            Clockwork.instance.CheckComplete();
        }

        // Final updates to socket
        transform.rotation = Quaternion.identity;
        placed = false;
        socket = null;
    }

    public void Shake() => StartCoroutine(ShakeCO());

    private IEnumerator ShakeCO() // Shake
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        Clockwork.instance.PlayRandomClip();
        float duration = .1f;
        float force = .01f;
        Vector3 shakePos;
        Transform gfx = transform.GetChild(0);

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            float randomX = Random.Range(-1 * force, 1 * force);
            float randomY = Random.Range(-1 * force, 1 * force);
            shakePos = new Vector3(randomX, randomY, 0);
            gfx.localPosition = shakePos;
            yield return waitForFixedUpdate;
        }

        gfx.localPosition = Vector3.zero;
    }
}
