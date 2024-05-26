using UnityEngine;

public class Puzzle2Singleton : MonoBehaviour
{
    public static Puzzle2Singleton Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }       
}