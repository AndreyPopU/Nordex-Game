using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
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
    }

    public virtual void Focus(Transform focus)
    {
        if (!interactable) return;

        finishedFocusing = false;
        Player.instance.focused = !Player.instance.focused;
        coreCollider.enabled = !Player.instance.focused;
        collision.enabled = !Player.instance.focused;
        Player.instance.coreCollider.enabled = !Player.instance.focused;
        Player.instance.rb.isKinematic = Player.instance.focused;
        Cursor.lockState = Player.instance.focused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Player.instance.focused ? true : false;
        StartCoroutine(FocusCO(focus));
    }

    private IEnumerator FocusCO(Transform focus)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        cam = Camera.main;
        //cam.transform.SetParent(null);

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
