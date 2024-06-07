using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class Tablet : MonoBehaviour
{
    public static Tablet instance;

    public GameObject tabletPanel;
    public bool paused;
    public TextMeshProUGUI taskText, descriptionText;
    [Header("Messages")]
    public GameObject chatBubble;
    public GameObject voiceBubble;
    public GameObject promptBubble;
    private AudioSource source;

    [Header("Test")]
    public string testMessage;
    public AudioClip testVoice;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Player.instance.focused) Pause();
    }

    public void Pause()
    {
        paused = !paused;
        Player.instance.canMove = !paused;
        Player.instance.rb.isKinematic = paused;
        tabletPanel.SetActive(paused);
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public void DisplayMessage(string chatMessage)
    {
        if (chatBubble.activeInHierarchy)
        // Show Chat message
        GameManager.instance.messagePrompt.desiredPosition = new Vector3(720, 260, 0);
        chatBubble.SetActive(true);
        chatBubble.GetComponentInChildren<TextMeshProUGUI>().text = chatMessage;
        Invoke("HideNotification", 5);
    }

    public void DisplayVoice()
    {
        if (source.isPlaying || voiceBubble.activeInHierarchy)
        {
            CancelInvoke("HideNotification");
            source.Stop();
        }

        // Show Voice message
        GameManager.instance.voicePrompt.desiredPosition = new Vector3(720, 260, 0);
        voiceBubble.SetActive(true);
        Invoke("HideNotification", 5);
    }

    private void HideNotification()
    {
        GameManager.instance.voicePrompt.desiredPosition = new Vector3(1200, 260, 0);
        GameManager.instance.messagePrompt.desiredPosition = new Vector3(1200, 260, 0);
    }

    public void UpdateTask(string task, string descrition)
    {
        taskText.text = task;
        descriptionText.text = descrition;
    }
}
