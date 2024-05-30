using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Tablet : MonoBehaviour
{
    public static Tablet instance;

    public GameObject tabletPanel;
    public bool paused;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
    }

    public void Pause()
    {
        paused = !paused;
        Player.instance.canMove = !paused;
        Player.instance.rb.isKinematic = paused;
        Cursor.lockState = paused ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = paused;
        tabletPanel.SetActive(paused);
    }
}
