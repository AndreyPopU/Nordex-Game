using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    public GameObject chatBubble;
    private AudioSource source;

    public AudioClip[] voiceMessages;
    public string[] chatMessages;

    public int voiceIndex;
    public int chatIndex;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            DisplayMessage();

        if (Input.GetKeyDown(KeyCode.V))
            PlayVoiceClip();
    }

    public void DisplayMessage()
    {
        if (chatIndex >= chatMessages.Length) return;

        // Play Message Notification

        print(chatMessages[chatIndex++]);

        // Create Chat Bubble

        // Fill it with text
    }

    public void PlayVoiceClip()
    {
        if (voiceIndex >= voiceMessages.Length) return;

        source.clip = voiceMessages[voiceIndex++];
        source.Play();
    }
}
