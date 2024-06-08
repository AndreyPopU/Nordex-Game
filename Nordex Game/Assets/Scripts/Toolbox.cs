using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Toolbox : Puzzle
{
    public MultiPuzzleTimerScript multiPuzzleTimerScript;

    public static Toolbox instance;

    public LayerMask mask;
    public Tool[] tools;
    public PlacementBox[] placements;
    public Animator animator;

    private BoxCollider completeCollider;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // Setup puzzle
        completeCollider = GetComponents<BoxCollider>()[1];
        tools = GetComponentsInChildren<Tool>();
        placements = GetComponentsInChildren<PlacementBox>();
    }

    public void CheckComplete()
    {
        // Find all overlaping colliders in toolbox
        completeCollider.enabled = true;
        Collider[] overlapingColliders = Physics.OverlapBox(completeCollider.bounds.center, completeCollider.bounds.size / 2, Quaternion.identity, mask);

        print(overlapingColliders.Length);

        // If all 61 colliders of tools are inside of puzzle it is complete
        if (overlapingColliders.Length != 61) return;

        // Completed
        multiPuzzleTimerScript.StopTimer(2);
        Debug.Log("Stopped");

        Chronometer.instance.loop();
        animator.enabled = true;
        Focus(Player.instance.playerCam.transform);
        interactable = false;
        completeCollider.enabled = false;
        source.Play();
    }
    public void PickUp()
    {
        // If puzzle is complete you can pick it up and move it to the next puzzle
        if (!interactable && transform.parent == null)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Tablet.instance.UpdateTask("Climb the ladder and fix the wind turbine spin mechanism.", "Put the cogs in their places, be careful with rusted sockets.");
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

        multiPuzzleTimerScript.StartTimer(2);
        Debug.Log("Started");

        // Setup tools
        coreCollider.enabled = !Player.instance.focused;
        for (int i = 0; i < tools.Length; i++)
            if (!tools[i].placed)
                tools[i].interactable = Player.instance.focused;

        GameManager.instance.rotatePrompt.desiredPosition = Player.instance.focused ? new Vector3(-800, 475, 0) : new Vector3(-1200, 475, 0);
    }
}
