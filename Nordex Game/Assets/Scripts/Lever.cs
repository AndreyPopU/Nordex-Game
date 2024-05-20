using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public ladder ladder;
    private Animator animator;
    public bool inrange;
    public BoxCollider boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (inrange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                animator.SetTrigger("Pull");
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
