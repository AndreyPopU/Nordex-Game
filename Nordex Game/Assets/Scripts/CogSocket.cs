using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CogSocket : PlacementBox
{
    public CogSocket head;
    public bool rusted;
    public bool clockwise;
    public bool running;
    public CogSocket[] frontNeighbours;
    public CogSocket[] backNeighbours;
    private float spinningForce;
    
    private void Start()
    {
        spinningForce = clockwise ? -75 : 75;
    }

    private void FixedUpdate()
    {
        if (running) transform.Rotate(transform.forward, spinningForce * Time.fixedDeltaTime);
    }

    public void Run()
    {
        if (!full && !head) running = false; // Stop running if empty

        CheckFront();
        CheckBack();
    }

    public void CheckFront()
    {
        // Check all front neighbour sockets
        foreach (CogSocket socket in frontNeighbours)
        {
            // If current socket is running, check if other sockets are full and start running
            if (running)
            {
                if (socket.full)
                {
                    if (socket.rusted && backNeighbours[0].running)
                    {
                        head.running = false;
                        head.CheckFront();
                        return;
                    }

                    // start running full neighbours
                    socket.running = true;
                    socket.Run(); // Check their neighbours
                }
            }
            else
            {
                socket.running = false;
                socket.CheckFront();
            }
        }
    }

    public void CheckBack()
    {
        // Check all back neighbour sockets
        foreach (CogSocket socket in backNeighbours)
        {
            // If connects with rusted socket
            if (socket.rusted && backNeighbours[0].running)
            {
                head.running = false;
                head.CheckFront();
                return;
            }

            // If not running check if neighbours are; Check if a neighbour is full and running
            if (socket.full && socket.running)
            {
                // start running if full
                if (full)
                {
                    running = true;
                    CheckFront();
                }
            }
        }
    }
}
