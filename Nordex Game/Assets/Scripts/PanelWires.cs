using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;

public class PanelWires : Puzzle
{
    public static PanelWires instance;

    public GameObject panel;
    public PlacementBox[] placements;
    public Wire[] wires;
    public Screw[] screws;

    private Vector3 panelPosition;
    private BoxCollider coreCollider;
    [HideInInspector] public BoxCollider panelCollider;
    public CanvasGroup canvas;

    private void Awake()
    {
        instance = this;
        coreCollider = GetComponent<BoxCollider>();
        panelCollider = GetComponents<BoxCollider>()[1];
        panelPosition = panel.transform.position;
        canvas.alpha = 0;
    }

    private void Update()
    {
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, panelPosition, 6 * Time.deltaTime);
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        if (Player.instance.focused) StartCoroutine(FadeCanvas(1));
        else StartCoroutine(FadeCanvas(0));

        coreCollider.enabled = !Player.instance.focused;
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.useGravity = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Player.instance.rb.velocity = Vector3.zero;

        for (int i = 0; i < wires.Length; i++)
            if (!wires[i].placed)
                wires[i].interactable = Player.instance.focused;

        for (int i = 0; i < screws.Length; i++)
        {
            if (!Player.instance.focused && !screws[i].screwed)
            {
                screws[i].Interact();
                screws[i].GFX.SetActive(true);
            }

            screws[i].interactable = Player.instance.focused;
        }

        // Close panel when unfocusing
        if (panel.transform.localPosition.x < 0) panelPosition += panel.transform.forward * 4 + panel.transform.right * .2f;
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < screws.Length; i++)
            if (screws[i].screwed) return;

        panelPosition += -panel.transform.forward * 4 - panel.transform.right * .2f;
        panelCollider.enabled = false;
    }

    public void CheckComplete()
    {
        for (int i = 0; i < wires.Length; i++)
        {
            if (wires[i].connectedIndex != wires[i].index) return;
        }

        // Complete
        print("Completed");
        Focus(Player.instance.playerCam.transform);
        interactable = false;
    }

    private IEnumerator FadeCanvas(float desire)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        if (canvas.alpha > desire)
        {
            while (canvas.alpha > desire)
            {
                canvas.alpha -= 2 * Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
        }
        else
        {
            if (canvas.alpha < desire)
            {
                while (canvas.alpha < desire)
                {
                    canvas.alpha += 2 * Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
            }
        }

        canvas.alpha = desire;
    }
}
