using System.Collections;
using UnityEngine;

public class Puzzle2ScaleObjectSmoothlyYScript : MonoBehaviour
{
    public float waitTime = 5.0f;
    public float scalingDuration = 2.0f;
    public float scaleFactor = 4.0f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; 
        StartCoroutine(ScaleAfterDelay());
    }

    public IEnumerator ScaleAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * scaleFactor, originalScale.z);
        float elapsedTime = 0.0f;

        while (elapsedTime < scalingDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scalingDuration);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void ResetToOriginalScale()
    {
        transform.localScale = originalScale; 
    }
}