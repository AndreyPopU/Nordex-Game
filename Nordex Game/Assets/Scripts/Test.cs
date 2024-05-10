using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            child.position = new Vector3(child.position.x * 100, child.position.y * 100, child.position.z * 100);
        }
    }
}
