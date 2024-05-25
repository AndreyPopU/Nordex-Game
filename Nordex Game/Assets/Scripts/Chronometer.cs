using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronometer : MonoBehaviour
{
    public static Chronometer instance;
    public float currenttime;
    public List<float> times;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy (gameObject); 
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
            loop();

        currenttime += Time.deltaTime;
    }

    public void loop ()
    {
        times.Add(currenttime);
        print(currenttime);
    }


}
