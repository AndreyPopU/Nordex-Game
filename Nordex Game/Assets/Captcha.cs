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

    private BoxCollider coreCollider;

    void Start()
    {
        instance = this;
        coreCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        
    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        coreCollider.enabled = !Player.instance.focused;

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

        selected.Add(_number);
        current += _number.value;
        _number.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;

        if (current == target)
        {
            foreach (CaptchaNumber number in selected)
                number.Lock();

            selected.Clear();
        }
        else if (current > target)
        {
            foreach (CaptchaNumber number in selected)
                number.ResetNumber();

            current = 0;
            selected.Clear();

            CheckComplete();
        }
    }

    public void RestartMinigame()
    {
        foreach (CaptchaNumber number in numbers)
            number.ResetNumber();

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
