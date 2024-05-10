using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseButton : MonoBehaviour
{
    public float speed;

    private Vector3 desiredPos;

    void Start()
    {
        desiredPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed);
    }

    private void OnMouseDown()
    {
        // Go back
        desiredPos += transform.forward * .1f;
    }

    private void OnMouseUp()
    {
        // Go front
        desiredPos -= transform.forward * .1f;
    }
}
