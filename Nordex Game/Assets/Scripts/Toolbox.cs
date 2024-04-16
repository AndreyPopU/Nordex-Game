using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Toolbox : Puzzle
{
    public static Toolbox instance;

    public LayerMask mask;
    public Tool[] tools;
    public PlacementBox[] placements;
    public Animator animator;

    private BoxCollider coreCollider;
    private BoxCollider completeCollider;

    void Awake()
    {
        instance = this;
        coreCollider = GetComponent<BoxCollider>();
        completeCollider = GetComponents<BoxCollider>()[1];
        tools = GetComponentsInChildren<Tool>();
        placements = GetComponentsInChildren<PlacementBox>();
    }

    public void CheckComplete()
    {
        Collider[] overlapingColliders = Physics.OverlapBox(completeCollider.bounds.center, completeCollider.bounds.size / 2, Quaternion.identity, mask);

        print(overlapingColliders.Length);

        if (overlapingColliders.Length != 32) return;

        // Completed
        animator.enabled = true;
        Focus(Player.instance.playerCam.transform);
        interactable = false;
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        coreCollider.enabled = !Player.instance.focused;
        for (int i = 0; i < tools.Length; i++)
            if (!tools[i].placed)
                tools[i].interactable = Player.instance.focused;
    }
}
