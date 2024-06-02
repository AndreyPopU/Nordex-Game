using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    public Vector3 desiredPosition;

    void Awake()
    {
        desiredPosition = transform.localPosition;
    }

    void Update()
    {
        if (Vector3.Distance(transform.localPosition, desiredPosition) > .1f) transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, 4 * Time.deltaTime);
    }

    public void SetDesiredPosition(Vector3 newPos)
    {
        desiredPosition = newPos;
    }
}
