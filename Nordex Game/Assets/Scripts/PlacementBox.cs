using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlacementItem item))
        {
            if (item.index == index)
            {
                // Snap
                item.interactable = false;
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.transform.position = transform.position;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
