using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [HideInInspector] public AudioSource source;
    public AudioClip completeClip;

    [Header("Camera Focus")]
    public Transform focusTransform;
    public float smoothing = .1f;
    public bool finishedFocusing = true;
    public bool interactable = true;

    private Camera cam;
    private Vector3 velocity;
    [HideInInspector] public BoxCollider coreCollider;
    [HideInInspector] public BoxCollider collision;

    public virtual void Start()
    {
        coreCollider = GetComponent<BoxCollider>();
        collision = GetComponents<BoxCollider>()[1];

        source = GetComponent<AudioSource>();
        source.clip = completeClip;
    }

    public virtual void Focus(Transform focus)
    {
        if (!interactable) return;

        // Setup player, cursor and camera
        finishedFocusing = false;
        Player.instance.focused = !Player.instance.focused;
        coreCollider.enabled = !Player.instance.focused;
        collision.enabled = !Player.instance.focused;
        Tablet.instance.promptBubble.SetActive(!Player.instance.focused);
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Cursor.lockState = Player.instance.focused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Player.instance.focused ? true : false;
        StartCoroutine(FocusCO(focus));
    }

    private IEnumerator FocusCO(Transform focus) // Gradually move and rotate the camera so it matches the focus point of the puzzle
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        cam = Camera.main;

        float focusTime = 1;
        while (focusTime > 0)
        {
            focusTime -= Time.deltaTime;
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, focus.position, ref velocity, smoothing);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, focus.rotation, smoothing);

            yield return waitForFixedUpdate;
        }

        finishedFocusing = true;
    }
}
