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
        PlayVoice(bossMessage1, bossMessage1, 0);

        MainMenuManager manager = FindObjectOfType<MainMenuManager>();
        if (manager)
        {
            man = FindObjectOfType<MainMenuManager>().man;
            Destroy(FindObjectOfType<MainMenuManager>().gameObject);
        }
        tutorialPrompt.desiredPosition = new Vector3(-740, 370, 0);
        Invoke("HideTutorial", 10);
    }

    public void StartGame() => FadePanel.instance.LoadScene("Teren", new Vector3(0, 2, -25));

    private void HideTutorial()
    {
        tutorialPrompt.desiredPosition = new Vector3(-1300, 370, 0);
        Destroy(tutorialPrompt.gameObject, 20);

        Invoke("PromptTablet", 1);
    }

    private void PromptTablet()
    {
        tabletPrompt.desiredPosition = new Vector3(-800, 475, 0);
        Invoke("HideTabletPrompt", 10);
        Destroy(tabletPrompt.gameObject, 20);
    }

    private void HideTabletPrompt()
    {
        tabletPrompt.desiredPosition = new Vector3(-1300, 475, 0);
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

    public void PlayVoice(AudioClip clipM, AudioClip clipF, float delay)
    {
        StartCoroutine(PlayVoiceCO(clipM, clipF, delay));
    }

    private IEnumerator PlayVoiceCO(AudioClip clipM, AudioClip clipF, float delay)
    {
        yield return new WaitForSeconds(delay);

        // If boss message
        if (clipM == clipF) Tablet.instance.DisplayVoice();

        GameObject holder = new GameObject();
        holder.AddComponent<AudioSource>();
        if (man) holder.GetComponent<AudioSource>().clip = clipM;
        else holder.GetComponent<AudioSource>().clip = clipF;
        holder.GetComponent<AudioSource>().Play();
        holder.transform.SetParent(Player.instance.transform);

        Destroy(holder, 20);
    }
}
