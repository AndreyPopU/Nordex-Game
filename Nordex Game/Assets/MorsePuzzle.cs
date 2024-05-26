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
}
