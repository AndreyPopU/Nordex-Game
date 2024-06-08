using UnityEngine;
using TMPro;

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

    // Start the timer for a specific puzzle
    public void StartTimer(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= startTimes.Length)
        {
            Debug.LogError("Invalid puzzle index");
            return;
        }

        startTimes[puzzleIndex] = Time.time;
        isTiming[puzzleIndex] = true;
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
            DisplayTime(puzzleIndex, endTime - startTimes[puzzleIndex]);
        }
    }

    // Display the elapsed time for a specific puzzle
    private void DisplayTime(int puzzleIndex, float elapsedTime)
    {
        switch (puzzleIndex)
        {
            case 0:
                timerTextPuzzle1.text = $"Wires - {elapsedTime:F2} seconds";
                break;
            case 1:
                timerTextPuzzle2.text = $"Maze - {elapsedTime:F2} seconds";
                break;
            case 2:
                timerTextPuzzle3.text = $"Toolbox - {elapsedTime:F2} seconds";
                break;
            case 3:
                timerTextPuzzle4.text = $"Clockwork - {elapsedTime:F2} seconds";
                break;
            case 4:
                timerTextPuzzle5.text = $"Frequency - {elapsedTime:F2} seconds";
                break;
            case 5:
                timerTextPuzzle6.text = $"Captcha - {elapsedTime:F2} seconds";
                break;
            case 6:
                timerTextPuzzle7.text = $"Morse - {elapsedTime:F2} seconds";
                break;
            default:
                Debug.LogError("Invalid puzzle index");
                break;
        }
    }
}