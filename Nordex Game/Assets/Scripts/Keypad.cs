using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keypad : Puzzle
{
    public static Keypad instance;
    public TextMeshProUGUI timerText;
    public Button[] hintButtons;
    public GameObject[] lockIcons;
    public float time = 0f;
    public float hintTime = 0f;
    public int hintIndex;
    public HintManager hintInstance;

    [Header("UI")]
    public CanvasGroup canvas;
    public TextMeshProUGUI guessField;

    private Vector3 startPos;

    //adding sound and light
    public lamp light1, light2;
    public AudioSource sound1, sound2;


    private void Awake() => instance = this; 

    private void Start()
    {
        startPos = transform.position;
        canvas.alpha = 0;
    }

    public void Update()
    {
        if (Player.instance.focused && Player.instance.puzzleInRange == this)
        {
            if (time > 0f) time -= Time.deltaTime;

            if (hintTime > 0f)
            {
                hintTime -= Time.deltaTime;

                TimeSpan timeSpan = TimeSpan.FromSeconds(hintTime);
                timerText.text = timeSpan.ToString(@"mm\:ss");
            }
            else
            {
                if (hintIndex < 3)
                {
                    hintButtons[hintIndex].interactable = true;
                    hintButtons[hintIndex].transform.GetChild(0).gameObject.SetActive(true);
                    lockIcons[hintIndex].SetActive(false);
                    hintIndex++;

                    if (hintIndex < 3) hintTime = 300f;
                    else
                    {
                        hintTime = 0f;
                        timerText.text = "00:00";
                    }
                }
            }
        }
    }

    public override void Focus(Transform focus)
    {
        //EventSystem eventSystem = EventSystem.current;
        //GameObject selectedGameObject = eventSystem.currentSelectedGameObject;

        //if (Player.instance.focused && selectedGameObject == guessField.gameObject) return; 

        base.Focus(focus);

        if (Player.instance.focused) StartCoroutine(FadeCanvas(1));
        else StartCoroutine(FadeCanvas(0));
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

    private IEnumerator FadeCanvas(float desire)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        if (canvas.alpha > desire)
        {
            while (canvas.alpha > desire)
            {
                canvas.alpha -= 2 * Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
        } 
        else
        {
            if (canvas.alpha < desire)
            {
                while (canvas.alpha < desire)
                {
                    canvas.alpha += 2 * Time.fixedDeltaTime;
                    yield return waitForFixedUpdate;
                }
            }
        }

        canvas.alpha = desire;
    }
}
