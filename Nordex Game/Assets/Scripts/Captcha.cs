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
    public List<CaptchaNumber> selected;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
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

        Focus(Player.instance.playerCam);
        interactable = false;
    }
}
