using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CaptchaNumber : MonoBehaviour
{
    public int value;
    public List<CaptchaNumber> neighbours;
    public bool interactable;

    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 desiredPosition;
    [HideInInspector] public TextMeshProUGUI text;
    private GameObject gfx;
    private Coroutine runningCo;
    private Vector3 desiredScale;
    private AudioSource source;

    void Start()
    {
        // Setup
        source = GetComponent<AudioSource>();
        gfx = transform.GetChild(0).gameObject;
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (value>= 0) text.text = value.ToString();
        startPos = transform.position;
        desiredPosition = startPos;
        desiredScale = Vector3.one * .2f;
    }

    private void Update()
    {
        // Update position and scale as desired
        transform.position = Vector3.Lerp(transform.position, desiredPosition, .1f);
        gfx.transform.localScale = Vector3.Lerp(gfx.transform.localScale, desiredScale, .1f);
    }

    private void OnMouseDown()
    {
        if (interactable) Captcha.instance.SelectNumber(this);
        
    }

    private void OnMouseEnter()
    {
        desiredScale = Vector3.one * .225f;
    }

    private void OnMouseExit()
    {
        desiredScale = Vector3.one * .2f;
    }

    public void Lock()
    {
        // Lock number
        SmartCoroutine(SpinCO());
        interactable = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.gray;
    }

    public void ResetNumber()
    {
        // Reset number
        text.color = Color.white;
        desiredPosition = startPos;
        interactable = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void SmartCoroutine(IEnumerator coroutine) 
    {
        // Make sure only one coroutine is running
        if (runningCo == null) runningCo = StartCoroutine(coroutine);
        else
        {
            StopCoroutine(runningCo);
            runningCo = StartCoroutine(coroutine);
        }
    }

    public IEnumerator ShakeCO() // Shake
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float elapsed = 0;
        float duration = .25f;
        Vector3 shakePos = Vector3.zero;

        while(elapsed < duration)
        {
            shakePos = new Vector3(Random.Range(-.01f, .01f), Random.Range(-.01f, .01f), 0);
            gfx.transform.localPosition = shakePos;
            elapsed += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        gfx.transform.localPosition = Vector3.zero;
        runningCo = null;
    }

    public IEnumerator SpinCO() // Spin
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        if (interactable)
        {
            while (gfx.transform.localRotation.y < 180)
            {
                gfx.transform.localRotation = Quaternion.Lerp(gfx.transform.localRotation, Quaternion.Euler(0, -180, 0), .1f);
                yield return waitForFixedUpdate;
            }
        }
        else
        {
            while (gfx.transform.localRotation.y > 0)
            {
                gfx.transform.localRotation = Quaternion.Lerp(gfx.transform.localRotation, Quaternion.identity, .1f);
                yield return waitForFixedUpdate;
            }
        }
        runningCo = null;
    }
}
