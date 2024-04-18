using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance.gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
