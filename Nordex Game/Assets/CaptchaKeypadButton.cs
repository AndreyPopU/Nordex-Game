using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CaptchaKeypadButton : MonoBehaviour
{
    public TextMeshProUGUI Textbox;
    public int number;
    public AudioClip Correct, Wrong, Pressed;

    public void GetPressedNumber()
    {
        if (Player.instance.focused == false) return;

        GetComponent<Animator>().SetTrigger("Pressed");
        AudioSource.PlayClipAtPoint(Pressed, transform.position);

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
                Textbox.text = "Correct";
                AudioSource.PlayClipAtPoint(Correct, transform.position);
                CaptchaKeypad.instance.interactable = false;
                Invoke("AskForCaptcha", 2);
            }
            else
            {
                Textbox.text = "Wrong";
                AudioSource.PlayClipAtPoint(Wrong, transform.position);
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
        CaptchaKeypad.instance.source.clip = null;
    }
}
