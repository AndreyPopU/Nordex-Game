using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Duration of the shake effect
    public float shakeDuration = 1.5f;

    // Magnitude of the shake effect
    public float shakeMagnitude = 0.02f;

    // Time interval between shakes
    public float shakeInterval = 5f;

    // Reference to the camera that will shake
    public Camera cameraToShake;

    // Original position of the camera
    private Vector3 originalPosition;

    void OnEnable()
    {
        if (cameraToShake == null)
        {
            Debug.LogError("CameraToShake is not assigned!");
            return;
        }

        // Store the original position of the camera
        originalPosition = cameraToShake.transform.localPosition;

        // Start the shake effect immediately
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