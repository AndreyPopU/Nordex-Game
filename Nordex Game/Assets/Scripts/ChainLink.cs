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
        current = path[index].position;
        transform.position = current;

        direction = next.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
    }

    void Update()
    {
        if (!socket.running) return;

        // Calculate Rotation
        direction = next.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

        //transform.LookAt(next);

        if (Vector3.Distance(transform.position, current) > .01f) transform.position = Vector3.MoveTowards(transform.position, current, speed * Time.deltaTime);
        else NextPath();
    }

    public void NextPath()
    {
        index++;
        if (index >= path.Length) index = 0;

        current = path[index].position;
    }
}
