using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButtons : MonoBehaviour
{
    public MultiPuzzleTimerScript multiPuzzleTimerScript;

    public TextMeshProUGUI Textbox;
    public int number;
    public AudioClip Correct, Wrong, Pressed;

    public void GetPressedNumber()
    {
        if (Player.instance.focused == false) return;

        if (Keypad.instance.time > 0) return;

        GetComponent<Animator>().SetTrigger("Pressed");
        AudioSource.PlayClipAtPoint(Pressed,transform.position);

        //10 = Clear Text
        if (number == 10)
        {
            Textbox.text = "";
            return;
        }
        //11 = Enter
        else if (number == 11)
        {
            if (Textbox.text == "34")
            {
                Textbox.text = "Correct";
                AudioSource.PlayClipAtPoint(Correct, transform.position);

                MultiPuzzleTimerScript.Instance.StopTimer(4);
                Debug.Log("Stopped");

                //making sounds and lights play
                Keypad.instance.light1.enabled = true;
                Keypad.instance.light2.enabled = true;
                Keypad.instance.sound1.Play();
                Keypad.instance.sound2.Play();
                Tablet.instance.UpdateTask("Go in the shed and calibrate the panels. Use the code 6874 to enter the room", "Enter the correct pattern.");
                Keypad.instance.source.Play();
                GameManager.instance.PlayVoice(Keypad.instance.bossClear, Keypad.instance.bossClear, 4);
                GameManager.instance.PlayVoice(Player.instance.calibrateM, Player.instance.calibrateF, 16);
                Chronometer.instance.loop();
            }
            else 
            {
                Textbox.text = "Wrong";
                AudioSource.PlayClipAtPoint(Wrong, transform.position);
                Invoke("ResetField", 5);
                Keypad.instance.time = 3f;
                Keypad.instance.Shake();
            }

            return;
        }

        if (Textbox.text.Length >= 4) return;

        Textbox.text = Textbox.text + number;
    }

    void OnMouseDown()
    {
        GetPressedNumber();
    }

    private void ResetField()
    {
        Textbox.text = string.Empty;
    }
}
