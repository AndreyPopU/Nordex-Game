using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Toolbox : Puzzle
{
    public static Toolbox instance;

    public LayerMask mask;
    public PlacementItem[] tools;
    public PlacementBox[] placements;
    public Animator animator;

    private BoxCollider coreCollider;
    private BoxCollider completeCollider;

    void Awake()
    {
        instance = this;
        coreCollider = GetComponent<BoxCollider>();
        completeCollider = GetComponents<BoxCollider>()[1];
        tools = GetComponentsInChildren<PlacementItem>();
        placements = GetComponentsInChildren<PlacementBox>();
    }

    void Update()
    {
        
    }

    public void CheckComplete()
    {
        Collider[] overlapingColliders = Physics.OverlapBox(completeCollider.bounds.center, completeCollider.bounds.size / 2, Quaternion.identity, mask);

        print("Overlapping colliders is " + overlapingColliders.Length);

        if (overlapingColliders.Length != 29) return;

        // Completed
        animator.enabled = true;
        Focus(Player.instance.playerCam.transform);
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
