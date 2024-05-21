using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButtons : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Textbox;
    [SerializeField] public int number;

    [SerializeField] 
    private float anthenaSoundDelay = 2f;

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
                Chronometer.instance.loop();

                StartCoroutine(AnthenaSoundsPlay());
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

    private IEnumerator AnthenaSoundsPlay()
    {
        while (true)
        {
            Keypad.instance.sound1.Play();
            yield return new WaitForSeconds(Keypad.instance.sound1.clip.length);
            Keypad.instance.sound1.Stop();

            yield return new WaitForSeconds(anthenaSoundDelay);

            Keypad.instance.sound2.Play();
            yield return new WaitForSeconds(Keypad.instance.sound2.clip.length);
            Keypad.instance.sound2.Stop();

            yield return new WaitForSeconds(anthenaSoundDelay);
        }
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
