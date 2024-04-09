using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CaptchaNumber : MonoBehaviour
{
    public int value;
    public List<CaptchaNumber> neighbours;
    public bool interactable;

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (value>= 0) text.text = value.ToString();
    }

    private void OnMouseDown()
    {
        if (interactable) Captcha.instance.SelectNumber(this);
    }

    public void Lock()
    {
        text.gameObject.SetActive(false);
        interactable = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.gray;
    }

    public void ResetNumber()
    {
        text.gameObject.SetActive(true);
        interactable = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
