using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class MultiPuzzleTimerScript : MonoBehaviour
{
    public static MultiPuzzleTimerScript Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public TextMeshProUGUI timerTextPuzzle1;
    public TextMeshProUGUI timerTextPuzzle2;
    public TextMeshProUGUI timerTextPuzzle3;
    public TextMeshProUGUI timerTextPuzzle4;
    public TextMeshProUGUI timerTextPuzzle5;
    public TextMeshProUGUI timerTextPuzzle6;
    public TextMeshProUGUI timerTextPuzzle7;

    private float[] startTimes = new float[7];
    private bool[] isTiming = new bool[7];
    private float[] elapsedTimes = new float[7];

    private readonly string[] puzzleNames = new string[]
    {
        "Wires",
        "Maze",
        "Toolbox",
        "Clockwork",
        "Frequency",
        "Captcha",
        "Morse"
    };

    async void Start()
    {
        // Initialize Unity Gaming Services
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    // Start the timer for a specific puzzle
    public void StartTimer(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= startTimes.Length)
        {
            Debug.LogError("Invalid puzzle index");
            return;
        }

        if (!isTiming[puzzleIndex])
        {
            startTimes[puzzleIndex] = Time.time;
            isTiming[puzzleIndex] = true;
        }
    }

    // Stop the timer for a specific puzzle and display the result
    public void StopTimer(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= startTimes.Length)
        {
            Debug.LogError("Invalid puzzle index");
            return;
        }

        if (isTiming[puzzleIndex])
        {
            float endTime = Time.time;
            isTiming[puzzleIndex] = false;
            elapsedTimes[puzzleIndex] = endTime - startTimes[puzzleIndex];
            DisplayTime(puzzleIndex, elapsedTimes[puzzleIndex]);
        }
    }

    // Display the elapsed time for a specific puzzle
    private void DisplayTime(int puzzleIndex, float elapsedTime)
    {
        switch (puzzleIndex)
        {
            case 0:
                timerTextPuzzle1.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 1:
                timerTextPuzzle2.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 2:
                timerTextPuzzle3.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 3:
                timerTextPuzzle4.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 4:
                timerTextPuzzle5.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 5:
                timerTextPuzzle6.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            case 6:
                timerTextPuzzle7.text = $"{puzzleNames[puzzleIndex]} - {elapsedTime:F2} seconds";
                break;
            default:
                Debug.LogError("Invalid puzzle index");
                break;
        }
    }

    // Method after finishing the game to send times to Unity Cloud Save
    public async Task SendElapsedTimesToCloud()
    {
        // Prepare a dictionary to send to Cloud Save
        Dictionary<string, object> data = new Dictionary<string, object>();

        // Add each elapsed time with descriptive names to the dictionary
        for (int i = 0; i < elapsedTimes.Length; i++)
        {
            data[$"{puzzleNames[i]}_Time"] = elapsedTimes[i];
        }

        try
        {
            // Send the data to Cloud Save
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("Elapsed times saved successfully to Unity Cloud.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data to Unity Cloud: {e.Message}");
        }
    }
}