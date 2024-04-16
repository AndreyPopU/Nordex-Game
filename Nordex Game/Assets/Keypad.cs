using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Keypad : Puzzle
{
    public static Keypad instance;
    public float time = 0f;

    private Vector3 startPos;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
    }

    public void Shake()
    {
        StartCoroutine(ShakeCO());
    }

    private IEnumerator ShakeCO()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float elapsed = 0;
        float duration = .25f;
        Vector3 shakePos = transform.position;

        while (elapsed < duration)
        {

            shakePos = new Vector3(startPos.x + Random.Range(-.01f, .01f), startPos.y + Random.Range(-.01f, .01f), startPos.z);
            transform.position = shakePos;
            elapsed += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        transform.position = startPos;
    }
}
