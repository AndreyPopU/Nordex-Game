using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogSocket : PlacementBox
{
    public bool clockwise;
    private float spinningForce;

    private void Start()
    {
        spinningForce = clockwise ? -75 : 75;
    }

    private void FixedUpdate()
    {
        transform.Rotate(transform.forward, spinningForce * Time.fixedDeltaTime);
    }
}
