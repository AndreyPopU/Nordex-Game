using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanelWires : Puzzle
{
    public GameObject panel;
    public PlacementItem[] wires;

    private BoxCollider coreCollider;

    private void Start()
    {
        coreCollider = GetComponent<BoxCollider>();
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);
        panel.SetActive(!Player.instance.focused);

        coreCollider.enabled = !Player.instance.focused;
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.useGravity = !Player.instance.focused;
        Player.instance.rb.isKinematic = !Player.instance.focused;
        Player.instance.rb.velocity = Vector3.zero;

        for (int i = 0; i < wires.Length; i++)
            if (!wires[i].placed)
                wires[i].interactable = Player.instance.focused;
    }
}
