using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireEnd : MonoBehaviour
{
    public Transform wire;

    void Update()
    {
        transform.position = wire.position;
        transform.up = wire.up;
    }
}
