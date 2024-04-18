using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject hintDescription;
    public bool descriptionVisible;

    private Coroutine runningCo;

    public void SmartCoroutine(IEnumerator coroutine)
    {
        if (runningCo == null) runningCo = StartCoroutine(coroutine);
        else
        {
            StopCoroutine(runningCo);
            runningCo = StartCoroutine(coroutine);
        }
    }

    public void RevealHint()
    {
        if (!descriptionVisible) SmartCoroutine(RevealHintCO());
        else SmartCoroutine(HideHintCO());
        descriptionVisible = !descriptionVisible;
    }

    private IEnumerator RevealHintCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float currentY = hintDescription.transform.localScale.y;

        while (hintDescription.transform.localScale.y < 1)
        {
            currentY += .1f;
            hintDescription.transform.localScale = new Vector3(1, currentY, 1);
            yield return waitForFixedUpdate;
        }

        hintDescription.transform.localScale = Vector3.one;
        runningCo = null;
    }

    private IEnumerator HideHintCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float currentY = hintDescription.transform.localScale.y;

        while (hintDescription.transform.localScale.y > 0)
        {
            currentY -= .1f;
            hintDescription.transform.localScale = new Vector3(1, currentY, 1);
            yield return waitForFixedUpdate;
        }

        hintDescription.transform.localScale = new Vector3(1, 0, 1);
        runningCo = null;
    }
}
