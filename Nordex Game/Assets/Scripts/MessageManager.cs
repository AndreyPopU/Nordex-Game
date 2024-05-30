using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    public bool voice;

    public GameObject chatBubble;
    public GameObject voiceBubble;
    public GameObject promptBubble;
    private AudioSource source;

    public AudioClip[] voiceMessages;
    public string[] chatMessages;

    public int voiceIndex;
    public int chatIndex;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            DisplayMessage();

        if (Input.GetKeyDown(KeyCode.G)) voice = !voice;
    }

    public void DisplayMessage()
    {
        if (chatIndex >= chatMessages.Length && !voiceBubble.activeInHierarchy && !chatBubble.activeInHierarchy) return;
        // Show voice message
        if (voice)
        {
            // Show Chat message
            voiceBubble.SetActive(true);
            Invoke("HideNotification", 5);
            // Play voice message
            PlayVoiceClip();
        }
        else
        {
            // Show Chat message
            chatBubble.SetActive(true);
            chatBubble.GetComponentInChildren<TextMeshProUGUI>().text = chatMessages[chatIndex++];
            Invoke("HideNotification", 5);
        }
    }

    public void PlayVoiceClip()
    {
        if (voiceIndex >= voiceMessages.Length) return;

        source.clip = voiceMessages[voiceIndex++];
        source.Play();
    }

    private void HideNotification()
    {
        voiceBubble.SetActive(false);
        chatBubble.SetActive(false);
    }
}
