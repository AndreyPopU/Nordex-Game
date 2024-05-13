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
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerStay (Collider other)    
    {
     if (other.GetComponent<Player>())
        {
            if (Input.GetButtonDown("Interact"))
            {
                SceneManager.LoadScene(scene);
                Player.instance.transform.position = spawnPosition;
            }
        }
    }
}
