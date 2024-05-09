using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        // Calculate Rotation
        Vector3 direction = transform.position - target.position;

        //Quaternion currentRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z);
        //Quaternion desiredRotation = Quaternion.LookRotation(direction, transform.up);
        //desiredRotation = Quaternion.Euler(0, 0, desiredRotation.eulerAngles.z);

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, transform.forward), 2 * Time.deltaTime);

        transform.LookAt(target);
    }
}
