using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class ResetButton : MonoBehaviour
{
    public TextMeshProUGUI morseText;

    public void GetPressedNumber()
    {
        //if (Player.instance.focused == false) return;

        GetComponent<Animator>().SetTrigger("Pressed");

        morseText.text = "";
        FindObjectOfType<MorseButton>().morseInput = "";
    }

    void OnMouseDown()
    {
        GetPressedNumber();
    }

}
