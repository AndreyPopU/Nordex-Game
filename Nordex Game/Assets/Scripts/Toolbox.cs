using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Toolbox : Puzzle
{
    public static Toolbox instance;
    public PlacementItem[] tools;
    public PlacementBox[] placements;
    public Animator animator;

    void Awake()
    {
        instance = this;
        tools = GetComponentsInChildren<PlacementItem>();
        placements = GetComponentsInChildren<PlacementBox>();
    }

    void Update()
    {
        
    }

    public void CheckComplete()
    {
        for (int i = 0; i < placements.Length; i++)
            if (!placements[i].full) return;

        // Completed
        animator.enabled = true;
        Focus(Player.instance.playerCam.transform);
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        for (int i = 0; i < tools.Length; i++)
            if (!tools[i].placed)
                tools[i].interactable = Player.instance.focused ? true : false;
    }
}
