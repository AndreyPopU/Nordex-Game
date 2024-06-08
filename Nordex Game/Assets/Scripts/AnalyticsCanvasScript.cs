using UnityEngine;

public class AnalyticsCanvasScript : MonoBehaviour
{
    public static AnalyticsCanvasScript Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}