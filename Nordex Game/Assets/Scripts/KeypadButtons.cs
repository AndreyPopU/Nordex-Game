using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButtons : MonoBehaviour
{
    public TextMeshProUGUI Textbox;
    public int number;

    public void GetPressedNumber()
    {
        if (Player.instance.focused == false) return;

        if (Keypad.instance.time > 0) return;

        GetComponent<Animator>().SetTrigger("Pressed");

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
                //making sounds and lights play
                Keypad.instance.light1.enabled = true;
                Keypad.instance.light2.enabled = true;
                Keypad.instance.sound1.Play();
                Keypad.instance.sound2.Play();
                Keypad.instance.completeSound.Play();
                Tablet.instance.UpdateTask("Go in the shed and calibrate the panels. Use the code 6874 to enter the room", "Enter the correct pattern.");
                Chronometer.instance.loop();
            }
            else 
            {
                Textbox.text = "Wrong";
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
