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


    public AudioSource source;
    public AudioClip unscrew;

    void Start()
    {
        GFX = transform.GetChild(0).gameObject;
        clockwork = Clockwork.instance;
        wires = PanelWires.instance;
        animator = GetComponent<Animator>();
        desiredPosition = transform.position;

        //sounds
        source = GetComponent<AudioSource>();
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

    public void Shake() => StartCoroutine(ShakeCO());

    private IEnumerator ShakeCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        float duration = .1f;
        float force = .01f;
        Vector3 originalPos = transform.position;
        Vector3 shakePos;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            float randomX = Random.Range(originalPos.x - 1 * force, originalPos.x + 1 * force);
            float randomY = Random.Range(originalPos.y - 1 * force, originalPos.y + 1 * force);
            shakePos = new Vector3(randomX, randomY, transform.position.z);
            transform.position = shakePos;
            yield return waitForFixedUpdate;
        }

        transform.position = originalPos;
    }

    private void OnMouseDown()
    {
        if (!screwed) return;

        Interact();
    }

    public void Interact()
    {
        if (!interactable) return;

        if (puzzle == Puzzle.clockwork && !Player.instance.hasToolbox) 
        {
            Shake();
            return;
        }
        //sounds
        source.clip = unscrew;
        source.Play();

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
