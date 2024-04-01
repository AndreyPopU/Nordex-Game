using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Device;

public class PanelWires : Puzzle
{
    public GameObject panel;
    public PlacementItem[] wires;
    public Screw[] screws;

    private Vector3 panelPosition;
    private BoxCollider coreCollider;
    [HideInInspector] public BoxCollider panelCollider;

    private void Start()
    {
        coreCollider = GetComponent<BoxCollider>();
        panelCollider = GetComponents<BoxCollider>()[1];
        panelPosition = panel.transform.position;
    }

    private void Update()
    {
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, panelPosition, 6 * Time.deltaTime);
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

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

        if (panel.transform.localPosition.y < 0) panelPosition += panel.transform.forward * 4 + panel.transform.right * .2f;
    }

    private void OnMouseDown()
    {
        for (int i = 0; i < screws.Length; i++)
            if (screws[i].screwed) return;

        panelPosition += -panel.transform.forward * 4 - panel.transform.right * .2f;
        panelCollider.enabled = false;
    }
}
