using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChainLink : MonoBehaviour
{
    public float speed, rotationSpeed;
    public Transform[] path;
    public Transform next;
    public Transform prev;
    private Vector3 direction;
    public Vector3 current;
    public int index;
    public CogSocket socket;

    private void Start()
    {
        // Start at your current position
        current = path[index].position;
        transform.position = current;

        // Face movement direction
        direction = next.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
    }

    void Update()
    {
        // If the socket that the chain is attached to isn't runnign - stop the chain from running
        if (!socket.running) return;

        // Calculate Rotation
        direction = next.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

        // Move the individual chain to the next destination in it's path
        if (Vector3.Distance(transform.position, current) > .01f) transform.position = Vector3.MoveTowards(transform.position, current, speed * Time.deltaTime);
        else NextPath(); // When next destination is reached, choose a new one
    }

    public void NextPath()
    {
        index++;
        if (index >= path.Length) index = 0;

        current = path[index].position;
    }
}
