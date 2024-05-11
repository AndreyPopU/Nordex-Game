using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbineConstantRotation : MonoBehaviour
{
    public bool mainTurbine;
    public float rotationSpeed;
    public Vector3 rotationDirection;
    // Update is called once per frame
    void Update()
    {
        if (!Player.instance.turbineSpin && mainTurbine) return;
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
    }
}
