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
                source.clip = PullSound;
                source.Play();
                Pulled = true; 
                ladder.GetComponent<Animator>().SetTrigger("GoDown");
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
