using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key: MonoBehaviour
{
    public AudioSource keyReceivedSound;

    public Transform holdtransform;
    
    public void PickUp()
    {
        holdtransform = GameObject.Find("HoldTransform").transform;
        transform.parent = holdtransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        Player.instance.haskey = true;
        keyReceivedSound.Play();
    }
}