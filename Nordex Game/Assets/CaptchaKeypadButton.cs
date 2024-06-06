using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaptchaKeypadButton : MonoBehaviour
{
    public TextMeshProUGUI Textbox;
    public int number;

    public void GetPressedNumber()
    {
        if (Player.instance.focused == false) return;

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
            // If code is correct - ask for captcha
            if (Textbox.text == "6874")
            {
                CaptchaKeypad.instance.completeSound.Play();
                Textbox.text = "Correct";
                CaptchaKeypad.instance.interactable = false;
                Invoke("AskForCaptcha", 2);
            }
            else
            {
                Textbox.text = "Wrong";
                Invoke("ResetField", 5);
                CaptchaKeypad.instance.Shake();
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

    private void AskForCaptcha()
    {
        Textbox.enableAutoSizing = true;
        Textbox.text = "Prove you are not a robot";
        CaptchaKeypad.instance.ActivateCaptcha();
    }
}
