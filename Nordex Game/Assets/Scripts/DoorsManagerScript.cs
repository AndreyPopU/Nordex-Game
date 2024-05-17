using UnityEngine;

public class DoorsManagerScript : MonoBehaviour
{
    [SerializeField]
    private Animator theDoor = null;

    [SerializeField]
    private bool openTrigger = false;
    [SerializeField]
    private bool closeTrigger = false;

    [SerializeField]
    private string doorOpen = "DoorOpen";
    [SerializeField]
    private string doorClose = "DoorClose";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                theDoor.Play(doorOpen, 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if (closeTrigger)
            {
                theDoor.Play(doorClose, 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}