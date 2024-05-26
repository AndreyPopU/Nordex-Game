using JetBrains.Annotations;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CaptchaKeypad : Puzzle
{
    public static CaptchaKeypad instance;

    public GameObject captchaPanel;

    private Vector3 desiredPosition;
    private Vector3 startPos;
    private bool reachedPosition = true;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else Destroy(transform.parent.gameObject);
    }

    public override void Start()
    {
        base.Start();
        startPos = transform.position;

        Invoke("DisableCaptcha", .2f);
    }

    private void DisableCaptcha()
    {
        // Disable captcha at first
        foreach (CaptchaNumber number in Captcha.instance.numbers)
            number.enabled = false;

        Captcha.instance.resetNumber.enabled = false;
        captchaPanel.transform.localPosition = Vector3.zero;
        captchaPanel.transform.localScale = new Vector3(0, 1, 1);
        captchaPanel.SetActive(false);
        reachedPosition = false;
    }

    public override void Focus(Transform focus)
    {
        // If captcha is required - focus to that instead
        if (Captcha.instance.gameObject.activeInHierarchy)
        {
            if (!Player.instance.focused) return;
            else
            {
                Captcha.instance.coreCollider.enabled = true;
                Captcha.instance.collision.enabled = true;
            }
        }

        base.Focus(focus);
    }

    private void Update()
    {
        if (reachedPosition) return;

        // Transition to captcha
        if (captchaPanel.transform.localScale.x < 1 && captchaPanel.activeInHierarchy)
            captchaPanel.transform.localScale = new Vector3(captchaPanel.transform.localScale.x + 2 * Time.deltaTime, 1, 1);

        if (captchaPanel.activeInHierarchy)
        {
            if (Vector3.Distance(captchaPanel.transform.position, desiredPosition) > 0.02f)
            {
                captchaPanel.transform.position = Vector3.Lerp(captchaPanel.transform.position, desiredPosition, 4 * Time.deltaTime);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredPosition - Vector3.forward, 4 * Time.deltaTime);
            }
            else
            {
                Captcha.instance.EnableNumbers();
                interactable = true;
                finishedFocusing = true;
                reachedPosition = true;
            }
        }
    }

    public void Shake() => StartCoroutine(ShakeCO());

    private IEnumerator ShakeCO() // Shake
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float elapsed = 0;
        float duration = .25f;
        Vector3 shakePos = transform.position;

        while (elapsed < duration)
        {
            shakePos = new Vector3(startPos.x + UnityEngine.Random.Range(-.01f, .01f), startPos.y + UnityEngine.Random.Range(-.01f, .01f), startPos.z);
            transform.position = shakePos;
            elapsed += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        transform.position = startPos;
    }

    public void ActivateCaptcha()
    {
        // Pull out captcha
        captchaPanel.gameObject.SetActive(true);
        desiredPosition = transform.position + transform.right * .7f;

        // Disable this puzzle
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        colliders[0].enabled = false;
        colliders[1].enabled = false;
        finishedFocusing = false;
    }
}
