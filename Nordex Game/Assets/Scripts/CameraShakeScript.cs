using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour
{
    public float shakeDuration = 1.5f;
    public float shakeMagnitude = 0.02f;
    public float shakeInterval = 5f;

    public Camera cameraToShake;

    private Vector3 originalPosition;

    void OnEnable()
    {
        if (cameraToShake == null)
        {
            Debug.LogError("CameraToShake is not assigned!");
            return;
        }

        originalPosition = cameraToShake.transform.localPosition;

        StartCoroutine(Shake());

        // Schedule repeated shakes
        InvokeRepeating("StartShake", shakeInterval, shakeInterval);
    }

    void OnDisable()
    {
        // Cancel the repeated shakes when the object is disabled
        CancelInvoke("StartShake");
    }

    void StartShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Randomly change the position of the camera
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            cameraToShake.transform.localPosition = originalPosition + randomOffset;

            // Increase the elapsed time
            elapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Reset the camera to its original position
        cameraToShake.transform.localPosition = originalPosition;
    }
}