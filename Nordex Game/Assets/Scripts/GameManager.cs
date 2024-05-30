using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool man;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance.gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }


    public void StartGame()
    {
        FadePanel.instance.LoadScene("Teren", new Vector3(0, 2, -25));
    }
}
