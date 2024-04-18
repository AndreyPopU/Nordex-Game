using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : Puzzle
{
    public static Keypad instance;
    public TextMeshProUGUI timerText;
    public Button[] hintButtons;
    public float time = 0f;
    public int hintIndex;

    [Header("UI")]
    public InputField guessField;

    private Vector3 startPos;

    private void Awake() => instance = this; 

    private void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        if (Player.instance.focused && Player.instance.puzzleInRange == this)
        {
            if (time > 0f)
            {
                time -= Time.deltaTime;

                TimeSpan timeSpan = TimeSpan.FromSeconds(time);
                timerText.text = timeSpan.ToString(@"mm\:ss");
            }
            else
            {
                if (hintIndex < 3)
                {
                    hintButtons[hintIndex++].interactable = true;

                    if (hintIndex < 3) time = 300f;
                    else
                    {
                        time = 0f;
                        timerText.text = "00:00";
                    }
                }
            }
        }
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);
    }

    public void Shake()
    {
        StartCoroutine(ShakeCO());
    }

    private IEnumerator ShakeCO()
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
}
