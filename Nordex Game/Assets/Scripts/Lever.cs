using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public ladder ladder;
    private Animator animator;
    public bool inrange;
    public BoxCollider boxCollider;
    public AudioClip PullSound, JamSound;
    public bool jammed;

    private AudioSource source;
    public bool Pulled;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (inrange)
        {
            if (Input.GetButtonDown("Interact") && Pulled == false)
            {
                animator.SetTrigger("Pull");
                
                //adding Pull sound
                if (!jammed) 
                {
                    source.clip = PullSound;
                    Pulled = true;
                    source.Play();
                    ladder.GetComponent<Animator>().SetTrigger("GoDown");
                }
                else source.clip = JamSound;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = false;
    }
}
