using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [Header("Camera Focus")]
    public Transform focusTransform;
    public float smoothing;
    public bool finishedFocusing = true;

    private Camera cam;
    private Vector3 velocity;

    private void Start()
    {
    }

    public void Focus(Transform focus)
    {
        finishedFocusing = false;
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
