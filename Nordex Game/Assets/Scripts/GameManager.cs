using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Prompt tutorialPrompt;
    public Prompt rotatePrompt;
    public Prompt messagePrompt;
    public Prompt voicePrompt;
    public bool man;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance.gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tutorialPrompt.desiredPosition = new Vector3(-740, 370, 0);
        Invoke("HideTutorial", 5);

    }
    private void Update()
    {

    }


    public void StartGame()
    {
        FadePanel.instance.LoadScene("Teren", new Vector3(0, 2, -25));
    }

    private void HideTutorial()
    {
        tutorialPrompt.desiredPosition = new Vector3(-1300, 370, 0);
        Destroy(tutorialPrompt.gameObject, 10);
    }
}
