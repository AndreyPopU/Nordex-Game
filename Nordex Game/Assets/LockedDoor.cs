using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool opened;
    public bool inrange;
    public bool locked;
    private Player player;
    public GameObject key;
    public  Animator animator;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (inrange)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!locked || locked && player.haskey) //key = open door
                {
                    animator.SetBool("Open", opened);
                    animator.SetTrigger("Trigger");
                    opened = !opened;

                    if (locked)
                    {
                        key.SetActive(false);
                        locked = false;
                    }
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (locked && !player.haskey)
            {
            }
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