using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsePuzzle : Puzzle
{
    public static MorsePuzzle instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    public override void Focus(Transform focus)
    {
        base.Focus(focus);

        // Now start morse because it is already in play 
        MultiPuzzleTimerScript.Instance.StartTimer(6);
        Debug.Log("Started");
    }
}
