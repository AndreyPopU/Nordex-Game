using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class lamp : MonoBehaviour
{
    private Light light;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        InvokeRepeating("Flickr", Random.Range(0.1f, 1), Random.Range(0.1f, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Flickr()
    {
        active =! active;
        light.enabled = active;

    }
}
