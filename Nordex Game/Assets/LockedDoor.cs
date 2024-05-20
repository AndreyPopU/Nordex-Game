using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool opened;
    public bool inrange;
    public bool locked;
    public  Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (inrange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!locked || locked && Player.instance.haskey) //key = open door
                {
                    if(GameObject.Find("HoldTransform")) GameObject.Find("HoldTransform").SetActive(false);
                    animator.SetBool("Open", opened);
                    animator.SetTrigger("Trigger");
                    opened = !opened;
                    locked = false;
                    Player.instance.haskey = false;

                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            inrange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            inrange = false;
        }
    }
}