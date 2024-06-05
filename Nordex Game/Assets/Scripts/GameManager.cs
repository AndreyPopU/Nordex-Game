using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool man;
    public AudioClip bossMessage1;

    [Header("Prompts")]
    public Prompt tutorialPrompt;
    public Prompt rotatePrompt;
    public Prompt messagePrompt;
    public Prompt voicePrompt;
    public Prompt tabletPrompt;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MainMenuManager manager = FindObjectOfType<MainMenuManager>();
        if (manager)
        {
            man = FindObjectOfType<MainMenuManager>().man;
            Destroy(FindObjectOfType<MainMenuManager>().gameObject);
        }
        tutorialPrompt.desiredPosition = new Vector3(-740, 370, 0);
        Invoke("HideTutorial", 5);
    }

    public void StartGame() => FadePanel.instance.LoadScene("Teren", new Vector3(0, 2, -25));

    private void HideTutorial()
    {
        tutorialPrompt.desiredPosition = new Vector3(-1300, 370, 0);
        Destroy(tutorialPrompt.gameObject, 10);

        Invoke("PromptTablet", 1);
    }

    private void PromptTablet()
    {
        tabletPrompt.desiredPosition = new Vector3(-800, 475, 0);
        Invoke("HideTabletPrompt", 5);
        Destroy(tabletPrompt.gameObject, 10);
    }

    private void HideTabletPrompt()
    {
        tabletPrompt.desiredPosition = new Vector3(-1300, 475, 0);

        Tablet.instance.DisplayVoice(bossMessage1);
    }

    public void SetGender(int gender)
    {
        switch (gender)
        {
            case 0: man = true; break;
            case 1: man = false; break;
            case 2: Application.Quit(); print("Kicked"); break;
        }
    }
}
