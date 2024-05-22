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

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (inrange)
        {
            if (Input.GetButtonDown("Interact") && !interacted)
            {
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
}
