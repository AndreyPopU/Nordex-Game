using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    public enum Puzzle { wires, clockwork }

    public Puzzle puzzle;
    public bool screwed = true;
    public bool interactable;
    public float screwSpeed = 3;

    [HideInInspector] public GameObject GFX;
    private Animator animator;
    private Vector3 desiredPosition;
    private Clockwork clockwork;
    private PanelWires wires;

    void Start()
    {
        GFX = transform.GetChild(0).gameObject;
        clockwork = Clockwork.instance;
        wires = PanelWires.instance;
        animator = GetComponent<Animator>();
        desiredPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, desiredPosition) > .01f)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, screwSpeed * Time.deltaTime);
        else
        {
            if (!screwed && GFX.activeInHierarchy) GFX.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!screwed) return;

        Interact();
    }

    public void Interact()
    {
        if (!interactable) return;

        if (puzzle == Puzzle.clockwork && !Player.instance.hasToolbox) return;

        animator.SetTrigger("Screw");
        screwed = !screwed;

        if (puzzle == Puzzle.wires)
        {
            desiredPosition += screwed ? transform.right : -transform.right;

            for (int i = 0; i < wires.screws.Length; i++)
            {
                if (wires.screws[i].screwed)
                    return;
            }
            wires.panelCollider.enabled = true;
        }
        else if (puzzle == Puzzle.clockwork)
        {
            desiredPosition += screwed ? -transform.forward : transform.forward;

            for (int i = 0; i < clockwork.screws.Length; i++)
            {
                if (clockwork.screws[i].screwed)
                    return;
            }
            clockwork.panelCollider.enabled = true;
        }
    }
}
