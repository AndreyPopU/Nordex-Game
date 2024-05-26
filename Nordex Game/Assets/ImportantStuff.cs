using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantStuff : MonoBehaviour
{
    public static ImportantStuff instance;

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
