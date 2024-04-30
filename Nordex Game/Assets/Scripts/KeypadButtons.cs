using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButtons : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Textbox;
    [SerializeField] public int number;


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
            }

            else 
            {
                Textbox.text = "Wrong";
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

    
}
