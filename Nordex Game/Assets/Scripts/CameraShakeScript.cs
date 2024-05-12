using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.01f;
    public float shakeInterval = 6f;

    public Camera cameraToShake;

    private Vector3 basePosition;

    void OnEnable()
    {
        if (cameraToShake == null)
        {
            Debug.LogError("CameraToShake is not assigned!");
            return;
        }

        basePosition = cameraToShake.transform.localPosition;

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
        // Update base position to current camera position before starting shake
        basePosition = cameraToShake.transform.localPosition;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Randomly change the position of the camera
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            cameraToShake.transform.localPosition = basePosition + randomOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset the camera to its base position
        cameraToShake.transform.localPosition = basePosition;
    }
}