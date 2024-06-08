using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MorseButton : MonoBehaviour
{
    public MultiPuzzleTimerScript multiPuzzleTimerScript;

    public string morseInput;
    public TextMeshProUGUI morseText;
    public bool holding;
    public float holdTime;
    public float resetTime = 5;
    public AudioClip bossLast;

    public float speed;

    private Vector3 desiredPos;

    public PlayerLeavingScript playerLeavingScript;

    void Start()
    {
        desiredPos = transform.position;
    }

    void Update()
    {
        if (!Player.instance.focused) return;

        transform.position = Vector3.Lerp(transform.position, desiredPos, speed);

        // Increase time
        if (holding) holdTime += Time.deltaTime;

        // If no input - reset string
        if (resetTime > 0) resetTime -= Time.deltaTime;
        else
        {
            resetTime = 5;
            morseInput = "";
            morseText.text = morseInput;
        }
    }

    private void OnMouseDown()
    {
        if (!Player.instance.focused) return;

        resetTime = 5;

        // Go back
        desiredPos -= transform.forward * .05f;

        // Start timer
        holding = true;
    }

    private void OnMouseUp()
    {
        if (!Player.instance.focused) return;

        // Go front
        desiredPos += transform.forward * .05f;

        // Check for timer
        holding = false;

        if (holdTime <= .25f) morseInput += ". ";
        else morseInput += "_ ";

        morseText.text = morseInput;

        holdTime = 0;

        CheckIfComplete();
    }

    public void CheckIfComplete()
    {
        if (morseInput.Length >= 24)
        {
            // If correct solution
            if (morseInput == "_ . _ . . . _ . _ . . . ")
            {
                morseText.text = "Correct";
                Chronometer.instance.loop();
                MorsePuzzle.instance.source.Play();
                GameManager.instance.PlayVoice(bossLast, bossLast, 1);

                MultiPuzzleTimerScript.Instance.StopTimer(6);
                Debug.Log("Stopped");

                playerLeavingScript.enabled = true;
            }
            else morseText.text = "Wrong";

            print(morseInput);
            print("_ . _ . . . _ . _ . . . ");

            morseInput = "";
        }
    }

}
