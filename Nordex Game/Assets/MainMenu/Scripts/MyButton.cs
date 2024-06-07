using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool disableOnClick;
    private bool mouseOver;
    Vector3 clickScale = new Vector2(1.4f, 1.4f);
    Vector3 enterScale = new Vector2(1.2f, 1.2f);
    Vector3 exitScale = new Vector2(1f, 1f);
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

    IEnumerator Click()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        while (transform.localScale.x < clickScale.x - .03f && mouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, clickScale, .5f);
            yield return waitForFixedUpdate;
        }

        while (transform.localScale.x > exitScale.x + .02f && mouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, exitScale, .5f);
            yield return waitForFixedUpdate;
        }

        runningCo = null;
    }

    IEnumerator Enter()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        while (transform.localScale.x < enterScale.x - .02f && mouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, enterScale, .5f);
            yield return waitForFixedUpdate;
        }

        runningCo = null;
    }

    IEnumerator Exit()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        while (transform.localScale.x > exitScale.x + .02f && !mouseOver)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, exitScale, .5f);
            yield return waitForFixedUpdate;
        }

        runningCo = null;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (disableOnClick) return;

        SmartCoroutine(Click());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        SmartCoroutine(Enter());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        SmartCoroutine(Exit());
    }
}
