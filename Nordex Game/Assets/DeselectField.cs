using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.DebugUI;

public class DeselectField : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI text;

    public void OnFocusInTextField(string str)
    {
        print("Selected");
        StartCoroutine(Deselect());
    }

    IEnumerator Deselect()
    {
        yield return new WaitForEndOfFrame();

        int position  = inputField.textComponent.text.Length - 1;
        inputField.caretPosition = position;
        inputField.selectionAnchorPosition = position;
        inputField.selectionFocusPosition = position;
        inputField.stringPosition = position;
        inputField.Select();
        text.text = inputField.textComponent.text;

    }
}
