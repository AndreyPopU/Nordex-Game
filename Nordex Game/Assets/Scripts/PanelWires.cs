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
    public BoxCollider panelCollider;
    public CanvasGroup canvas;
    public Lever lever;
    public BoxCollider blockingCollider;
    public GameObject light1, light2;
    public bool mechanismWorks;

    public AudioClip electricityOnSound;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        panelPosition = panel.transform.position;
        canvas.alpha = 0;
    }

    public override void Start()
    {
        base.Start();

        Tablet.instance.UpdateTask("Enter the turbine and figure out what is wrong.", "Search for problems that require fixing in the turbine.");
    }

    private void Update()
    {
        // Move panel to desired position
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, panelPosition, 6 * Time.deltaTime);
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        if (Player.instance.focused) StartCoroutine(FadeCanvas(1));
        else StartCoroutine(FadeCanvas(0));

        Player.instance.coreCollider.enabled = !Player.instance.focused;
        blockingCollider.enabled = !Player.instance.focused;
        Player.instance.rb.useGravity = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Player.instance.rb.velocity = Vector3.zero;

        // Update wires
        for (int i = 0; i < wires.Length; i++)
            if (!wires[i].placed)
                wires[i].interactable = Player.instance.focused;

        // Update screws
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
        // If all screws are unscrewed, take off the panel
        for (int i = 0; i < screws.Length; i++)
            if (screws[i].screwed) return;

        panelPosition += -panel.transform.forward * 4 - panel.transform.right * .2f;
        panelCollider.enabled = false;
    }

    public void CheckComplete()
    {
        // If all wires are connected to the correct socket, complete the puzzle
        for (int i = 0; i < wires.Length; i++)
        {
            if (wires[i].connectedIndex != wires[i].index) return;
        }

        // Complete
        print("Completed");
        light1.SetActive(true); light2.SetActive(true);
        Chronometer.instance.loop();
        Focus(Player.instance.playerCam.transform);
        interactable = false;
        AudioSource.PlayClipAtPoint(electricityOnSound, transform.position);
        source.Play();

        if (mechanismWorks)
        {
            lever.jammed = false;
            lever.GetComponent<Animator>().SetBool("Jamed", false);
            lever.boxCollider.enabled = true;
        }
    }

    private IEnumerator FadeCanvas(float desire) // UI smooth appear
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
