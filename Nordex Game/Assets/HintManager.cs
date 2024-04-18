using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public string description;
    public Color active, inactive;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI descriptionText;
    public Image hintTab;
    public Sprite activeUI; 

    public void EnableHint()
    {
        transform.parent.parent.GetComponent<Animator>().SetTrigger("activate");
        if (Keypad.instance.hintInstance != null) buttonText.color = inactive;

        Keypad.instance.hintInstance = this;

        descriptionText.text = description;
        hintTab.sprite = activeUI;
        buttonText.color = active;
}
}
