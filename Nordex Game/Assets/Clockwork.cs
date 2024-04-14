using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clockwork : Puzzle
{
    public static Clockwork instance;

    [Header("Time")]
    public float timeLeft = 10;
    public bool timeActive;
    public Slider timeSlider;

    [Header("Puzzle")]
    public GameObject panel;
    public PlacementBox[] sockets;
    public Cog[] cogs;
    public Screw[] screws;

    private Vector3 panelPosition;
    private BoxCollider coreCollider;
    private float baseTimeLeft;
    [HideInInspector] public BoxCollider panelCollider;

    private void Awake()
    {
        instance = this;
        coreCollider = GetComponent<BoxCollider>();
        panelCollider = GetComponents<BoxCollider>()[1];
        panelPosition = panel.transform.position;
        baseTimeLeft = timeLeft;
    }

    private void Update()
    {
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, panelPosition, 6 * Time.deltaTime);

        if (!timeActive) return;

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
        }
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        coreCollider.enabled = !Player.instance.focused;
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.useGravity = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Player.instance.rb.velocity = Vector3.zero;

        if (!timeActive) timeLeft = baseTimeLeft; // Reset time

        for (int i = 0; i < cogs.Length; i++)
            if (!cogs[i].placed)
                cogs[i].interactable = Player.instance.focused;

        for (int i = 0; i < screws.Length; i++)
        {
            if (!Player.instance.focused && !screws[i].screwed)
            {
                screws[i].Interact();
                screws[i].GFX.SetActive(true);
            }

            screws[i].interactable = Player.instance.focused;
        }

        if (panel.transform.localPosition.x < -1) panelPosition += panel.transform.right * 4 + panel.transform.up * .2f;
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < screws.Length; i++)
            if (screws[i].screwed) return;

        timeActive = true;
        panelPosition += -panel.transform.right * 4 - panel.transform.up * .2f;
        panelCollider.enabled = false;
    }

    public void CheckComplete()
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            if (!sockets[i].full) return;
        }

        // Complete
        print("Completed");
        Focus(Player.instance.playerCam.transform);
        interactable = false;
    }
}
