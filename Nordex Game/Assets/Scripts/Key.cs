using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform holdtransform;
    public bool inrange;
    public bool key;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            inrange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            inrange = false;
        }

    }
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inrange)
        {

            transform.parent = holdtransform;
            transform.localRotation = Quaternion.Euler(0, 90, 90);
            transform.localPosition = Vector3.zero;

            if (key)
            {
                FindObjectOfType<Player>().haskey = true;
                transform.localRotation = Quaternion.Euler(0, 0, 0);

            }
        }
    }
}
