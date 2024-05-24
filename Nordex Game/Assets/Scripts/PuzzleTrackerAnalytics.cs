using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class PuzzleTrackerAnalytics : MonoBehaviour
{
    private float startTime;

    private string currentPuzzle;

    public static PuzzleTrackerAnalytics instance;

    // Change Environment to "build"
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (!Application.isEditor) 
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("build");
            UnityServices.InitializeAsync(options);
        }
        else UnityServices.InitializeAsync();

        DontDestroyOnLoad(gameObject);
    }

    // Call when a player starts a puzzle
    public void AnalyticsStartPuzzle(string puzzleName)
    {
        currentPuzzle = puzzleName;
        startTime = Time.time;
        Debug.Log($"Started puzzle: {puzzleName} at time: {startTime}");
    }

    // Call when a player finishes a puzzle
    public void AnalyticsEndPuzzle()
    {
        if (string.IsNullOrEmpty(currentPuzzle))
        {
            Debug.LogWarning("No puzzle is currently being tracked.");
            return;
        }

        float endTime = Time.time;
        float duration = endTime - startTime;

        Debug.Log($"Finished puzzle: {currentPuzzle} at time: {endTime}, duration: {duration} seconds");

        // Send the custom event to Unity Analytics
        Analytics.CustomEvent("PuzzleCompleted", new Dictionary<string, object>
        {
            { "puzzleName", currentPuzzle },
            { "duration", duration }
        });

        // Reset the current puzzle
        currentPuzzle = null;
    }
}