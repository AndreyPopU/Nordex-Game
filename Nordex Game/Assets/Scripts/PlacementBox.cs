using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    public int index;
    public bool full;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out PlacementItem item))
    //    {
    //        if (item.index == index && !item.dragged)
    //        {
    //            // Snap
    //            item.interactable = false;
    //            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //            item.transform.position = transform.position;
    //            full = true;
    //            Toolbox.instance.CheckComplete();
    //        }
    //    }
    //}
}
