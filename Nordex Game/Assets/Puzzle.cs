using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [Header("Camera Focus")]
    public Transform focusTransform;
    public float smoothing;

    private Camera cam;

    private void Start()
    {
    }

    public void Focus(Transform focus)
    {
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
            cam.transform.position = Vector3.Lerp(cam.transform.position, focus.position, smoothing);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, focus.rotation, smoothing);

            yield return waitForFixedUpdate;
        }
    }
}
