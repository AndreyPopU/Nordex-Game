using UnityEngine;

public class RestartPuzzle2Script : MonoBehaviour
{
    public GameObject puzzle2;

    public void OnButtonClick()
    {
        puzzle2.SetActive(false);
        puzzle2.SetActive(true);
    }
}