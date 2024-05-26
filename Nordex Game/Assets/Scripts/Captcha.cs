using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Captcha : Puzzle
{
    public static Captcha instance;

    public int target;
    public int current;
    public CaptchaNumber[] numbers;
    public CaptchaNumber resetNumber;
    public List<CaptchaNumber> selected;
    public LockedDoor door;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!interactable)
        {
            if (Vector3.Distance(transform.parent.localPosition, Vector3.zero) > 0.02f)
            {
                transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, Vector3.zero, 4 * Time.deltaTime);

                if (transform.parent.localScale.x > 0)
                    transform.parent.localScale = new Vector3(transform.parent.localScale.x - 1 * Time.deltaTime, 1, 1);

                
            }
            else
            {
                gameObject.SetActive(false);
                FindObjectOfType<CaptchaKeypadButton>().Textbox.text = string.Empty;
                CaptchaKeypad.instance.coreCollider.enabled = false;
            }
        }
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        foreach (CaptchaNumber number in numbers)
            number.interactable = Player.instance.focused;
    }

    public void SelectNumber(CaptchaNumber _number)
    {
        // If reset button is clicked - reset
        if (_number.value < 0)
        {
            RestartMinigame();
            return;
        }

        // If you already contain the number, unselect it
        if (selected.Contains(_number))
        {
            _number.ResetNumber();
            selected.Remove(_number);
            current -= _number.value;
            return;
        }

        // If last selected square doesn't contain the newly selected square as one of it's neighbours, return
        if (selected.Count > 0 && !selected[selected.Count - 1].neighbours.Contains(_number)) return;

        if (selected.Count > 0) selected[selected.Count - 1].HighlightNeighbours(false);
        _number.text.color = Color.black;
        _number.desiredPosition = _number.startPos + _number.transform.forward * .05f;
        selected.Add(_number);
        current += _number.value;
        _number.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        _number.Highlight(true);
        _number.HighlightNeighbours(true);

        if (current == target)
        {
            foreach (CaptchaNumber number in selected)
                number.Lock();

            current = 0;
            selected.Clear();

            CheckComplete();
        }
        else if (current > target)
        {
            foreach (CaptchaNumber number in selected)
            {
                number.ResetNumber();
                number.SmartCoroutine(number.ShakeCO());
            }
            current = 0;
            selected.Clear();
        }
    }

    public void RestartMinigame()
    {
        foreach (CaptchaNumber number in numbers)
        {
            if (!number.interactable) number.SmartCoroutine(number.SpinCO());
            number.ResetNumber();
            number.text.gameObject.SetActive(true);
        }

        current = 0;
        selected.Clear();
    }

    public void CheckComplete()
    {
        foreach (CaptchaNumber number in numbers)
        {
            if (number.interactable) return;
        }
        Chronometer.instance.loop();

        Focus(Player.instance.playerCam);
        interactable = false;
        door.locked = false;

        foreach (CaptchaNumber number in numbers)
            number.enabled = false;

        resetNumber.enabled = false;
    }

    public void EnableNumbers()
    {
        foreach (CaptchaNumber number in numbers)
            number.enabled = true;

        resetNumber.enabled = true;
        coreCollider.enabled = false;
        collision.enabled = false;
        CaptchaKeypad.instance.coreCollider.enabled = false;
        CaptchaKeypad.instance.collision.enabled = false;
        Invoke("RestartMinigame", .1f);
    }
}
