using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using static UnityEditor.PlayerSettings;

public class CustomTMPInputField : TMP_InputField
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Override OnSelect to prevent selecting all text when the InputField is clicked
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // This will prevent the text from being selected when the InputField is selected
        DeactivateSelection();
    }

    // Override OnPointerClick to place the caret at the clicked position
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // Place caret at the cursor position instead of selecting all text
        int position = GetCharacterIndexFromPosition(eventData);
        caretPosition = position;
        selectionAnchorPosition = position;
        selectionFocusPosition = position;
    }

    // Get character index from the position of the pointer
    private int GetCharacterIndexFromPosition(PointerEventData eventData)
    {
        if (textComponent == null)
        {
            return 0;
        }

        Camera cam = null;
        if (textComponent.canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
            (textComponent.canvas.renderMode == RenderMode.ScreenSpaceCamera && textComponent.canvas.worldCamera == null))
        {
            cam = null;
        }
        else
        {
            cam = textComponent.canvas.worldCamera;
        }

        return TMP_TextUtilities.GetCursorIndexFromPosition(textComponent, eventData.position, cam);
    }

    // Method to deactivate text selection
    private void DeactivateSelection()
    {
        if (string.IsNullOrEmpty(this.text))
        {
            this.caretPosition = 0;
        }
        else
        {
            this.caretPosition = this.text.Length;
        }
        this.selectionAnchorPosition = this.caretPosition;
        this.selectionFocusPosition = this.caretPosition;
        print(caretPosition);
        print(caretPosition > m_TextComponent.textInfo.characterCount - 1);
    }
}
