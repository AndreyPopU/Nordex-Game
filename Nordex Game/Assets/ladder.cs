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
   
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (inrange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(LoadSceneASinc());
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
    public IEnumerator LoadSceneASinc ()
    {
        YieldInstruction wait = new WaitForFixedUpdate();

        Player.instance.rb.isKinematic = true;

        // Fade out
        while (FadePanel.instance.group.alpha < 1)
        {
            FadePanel.instance.group.alpha += .01f;
            yield return null;
        }

        // Start loading scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        // If fully faded out && scene is ready to be activated
        while (FadePanel.instance.group.alpha < 1 || operation.progress < .9f)
        {
            yield return null;
        }

        // Activate the scene
        operation.allowSceneActivation = true;

        // Setup player
        Player.instance.transform.position = spawnPosition;
        FadePanel.instance.StartCoroutine(FadePanel.instance.FadeOut());
    }
}
