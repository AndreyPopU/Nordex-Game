using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public bool waitForInput = true;
    public GameObject genderPanel;
    public bool man;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (waitForInput && Input.anyKeyDown)
        {
            genderPanel.SetActive(true);
            waitForInput = false;
        }
    }

    public void StartGame() => FadePanel.instance.LoadScene("Teren", new Vector3(0, 2, -25));

    public void QuitGame() => Application.Quit();

    public void SetGender(bool gender)
    {
        man = gender;
    }
}
