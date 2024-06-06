using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public bool opened;
    public bool inrange;
    public bool locked;
    public bool needKey;
    public  Animator animator;
    public AudioClip open, close, locksound, unlock;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (inrange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (locked && Player.instance.haskey)
                {
                    source.clip = unlock;
                    source.Play();

                    locked = false;
                }

                if (!locked || locked && Player.instance.haskey && needKey) //key = open door
                {
                    if(GameObject.Find("HoldTransform")) GameObject.Find("HoldTransform").SetActive(false);
                    animator.SetBool("Open", opened);
                    animator.SetTrigger("Open");
                    opened = !opened;
                    locked = false;
                    Player.instance.haskey = false;
                    source.clip = opened ? open : close;
                    source.Play();
                }
                else
                {
                    source.clip = locksound;
                    source.Play();
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