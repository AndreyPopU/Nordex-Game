using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireStretch : MonoBehaviour
{
    public Transform startPivot, endPivot;
    public Transform wireEnd;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        UpdateScale();
    }

    void Update()
    {
        if (endPivot.transform.hasChanged) UpdateScale();
    }

    void UpdateScale()
    {
        float distance = Vector3.Distance(startPivot.position, endPivot.position); 
        transform.localScale = new Vector3(initialScale.x, distance, initialScale.z);

        Vector3 middlePoint = (startPivot.position + endPivot.position) / 2;
        transform.position = middlePoint;

        Vector3 rotationDirection = (endPivot.position - startPivot.position);
        transform.up = rotationDirection;
        wireEnd.position = endPivot.position;
        wireEnd.up = rotationDirection;
    }
}
