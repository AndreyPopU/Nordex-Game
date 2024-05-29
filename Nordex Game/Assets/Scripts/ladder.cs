using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ladder : MonoBehaviour
{
    private Animator animator;
    public string scene;
    public Vector3 spawnPosition;
    public bool inrange;
    public bool interacted;
    private AudioSource audioSource;
    public AudioClip godown, climb;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (inrange)
        {
            if (Input.GetButtonDown("Interact") && !interacted)
            {
                audioSource.clip = climb;
                audioSource.Play();
                FadePanel.instance.LoadScene(scene, spawnPosition);
                interacted = true;
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
    public void GoDown ()
    {
        audioSource.clip = godown;
        audioSource.Play();
    }
}

