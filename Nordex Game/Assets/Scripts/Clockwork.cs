using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Clockwork : Puzzle
{
    public static Clockwork instance;

    public Transform toolboxTransform;
    [Header("Time")]
    public float timeLeft = 30;
    public bool timeActive;
    public Slider timeSlider;

    [Header("Puzzle")]
    public GameObject panel;
    public CogSocket[] sockets;
    public Cog[] cogs;
    public Screw[] screws;

    [Header("Variants")]
    public SocketVariant[] variants;
    public int index;

    [Header("Variants")]
    public AudioClip[] clips;
    private AudioSource audioSource;

    private Vector3 panelPosition;
    private float baseTimeLeft;
    public BoxCollider panelCollider;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        foreach (Cog c in cogs)
        {
            c.AddComponent<AudioSource>();  
        }

        // Setup
        panelPosition = panel.transform.position;
        baseTimeLeft = timeLeft;
        audioSource = GetComponent<AudioSource>();

        variants[index].gameObject.SetActive(true);
    }

    private void Update()
    {
        // Deal with panel
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, panelPosition, 6 * Time.deltaTime);

        if (!timeActive) return;

        // Slider countdown
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeSlider.value = timeLeft;
        }
        else
        {
            print("Time ran out!");
            foreach (Cog cog in cogs)
                cog.ResetPos();

            timeLeft = baseTimeLeft;
            ActivateVariant();
            CameraManager.instance.Shake(.2f, .1f);
        }
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        // If player has toolbox on him - place it to the side
        if (Camera.main.transform.childCount > 0 && Camera.main.transform.GetChild(0) != null)
        {
            Transform toolbox = Camera.main.transform.GetChild(0);
            toolbox.SetParent(toolboxTransform);
            toolbox.transform.localPosition = Vector3.zero;
            toolbox.transform.localRotation = Quaternion.identity;
            toolbox.GetComponent<Toolbox>().collision.enabled = false;
            toolbox.GetComponent<Toolbox>().enabled = false;
        }

        coreCollider.enabled = !Player.instance.focused;
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.useGravity = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Player.instance.rb.velocity = Vector3.zero;

        // Reset time
        if (!Player.instance.focused)
        {
            timeActive = false;
            timeLeft = baseTimeLeft;
        }

        // Setup Cogs
        for (int i = 0; i < cogs.Length; i++)
            if (!cogs[i].placed)
                cogs[i].interactable = Player.instance.focused;

        // Setup Screws
        for (int i = 0; i < screws.Length; i++)
        {
            if (!Player.instance.focused && !screws[i].screwed)
            {
                screws[i].Interact();
                screws[i].GFX.SetActive(true);
            }

            screws[i].interactable = Player.instance.focused;
        }

        // Setup Panel
        if (panel.transform.localPosition.x < -1) panelPosition += panel.transform.right * 4 + panel.transform.up * .2f;
    }

    private void OnMouseDown()
    {
        // If not all screws are unscrewed return;
        for (int i = 0; i < screws.Length; i++)
            if (screws[i].screwed) return;

        // Activate time and move panel out of the way
        timeActive = true;
        panelPosition += -panel.transform.right * 4 - panel.transform.up * .2f;
        panelCollider.enabled = false;
    }

    public void CheckComplete()
    {
        // If all sockets are full and mechanism is running - consider it complete
        for (int i = 0; i < sockets.Length; i++)
        {
            if (!sockets[i].full || !sockets[i].running) return;
        }

        // Complete
        print("Completed");
        Chronometer.instance.loop();
        timeLeft = 999999;
        Focus(Player.instance.playerCam.transform);
        interactable = false;
        Player.instance.turbineSpin = true;
        Tablet.instance.UpdateTask("Go back to the company building", "Return to the pick up truck.");
    }

    public void ActivateVariant()
    {
        // Update variant
        variants[index].gameObject.SetActive(false);

        index++;
        if (index >= variants.Length) index = 0;
        variants[index].gameObject.SetActive(true);

        sockets = variants[index].correctSockets;
    }

    public void PlayRandomClip()
    {
        // Player random sound when clockwork gets jammed
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
