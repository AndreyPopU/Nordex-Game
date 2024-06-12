using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public Transform[] path;
    public int index;
    public Vector3 current;
    public float speed = .125f;
    public CogSocket socket;

    private void Awake()
    {
        // Start at your current position
        current = path[index].position;
        transform.position = current;
    }

    void Update()
    {
        // If the socket that the chain is attached to isn't runnign - stop the chain from running
        if (!socket.running) return;

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
