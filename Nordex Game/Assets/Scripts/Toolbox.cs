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

    private BoxCollider completeCollider;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        completeCollider = GetComponents<BoxCollider>()[1];
        tools = GetComponentsInChildren<Tool>();
        placements = GetComponentsInChildren<PlacementBox>();
    }

    public void CheckComplete()
    {
        completeCollider.enabled = true;
        Collider[] overlapingColliders = Physics.OverlapBox(completeCollider.bounds.center, completeCollider.bounds.size / 2, Quaternion.identity, mask);

        print(overlapingColliders.Length);

        if (overlapingColliders.Length != 38) return;

        // Completed
        Chronometer.instance.loop();
        animator.enabled = true;
        Focus(Player.instance.playerCam.transform);
        interactable = false;

    }
    public void PickUp()
    {
        if (!interactable)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Player.instance.hasToolbox = true;
                transform.SetParent(Camera.main.transform);
                transform.localPosition = new Vector3(0.4f, -0.3f, 0.3f);
                transform.localRotation = Quaternion.Euler(new Vector3(-90,0,0));
            }
        }
    }
    private void Update()
    {
        PickUp();
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
