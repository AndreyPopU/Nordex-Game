using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public Transform[] path;
    public int index;
    public Vector3 current;
    public float speed = .125f;
    public CogSocket socket;

    private void Awake()
    {
        current = path[index].position;
        transform.position = current;
    }

    void Update()
    {
        if (!socket.running) return;

        if (Vector3.Distance(transform.position, current) > .01f) transform.position = Vector3.MoveTowards(transform.position, current, speed * Time.deltaTime);
        else NextPath();
    }

    public void NextPath()
    {
        index++;
        if (index >= path.Length) index = 0;

        current = path[index].position;
    }
}
