using System.Collections;
using UnityEngine;

public class ScaleObjectSmoothlyYScript : MonoBehaviour
{
    public float waitTime = 5.0f;
    public float scalingDuration = 2.0f;

    // Scale factor for Y axis
    public float scaleFactor = 4.0f;

    void Start()
    {
        StartCoroutine(ScaleAfterDelay());
    }

    private IEnumerator ScaleAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * scaleFactor, originalScale.z);

        float elapsedTime = 0.0f;

        // Perform the scaling over the duration
        while (elapsedTime < scalingDuration)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Interpolate between the original and target scale
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scalingDuration);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final scale is set
        transform.localScale = targetScale;
    }
}