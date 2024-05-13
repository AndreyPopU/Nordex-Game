using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public ladder ladder;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (Input.GetButtonDown("Interact"))
            {
                animator.SetTrigger("Pull");
                ladder.GetComponent<Animator>().SetTrigger("GoDown");
            }
        }
    }
}
