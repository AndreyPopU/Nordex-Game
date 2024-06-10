using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    public bool inRange;
    public Key keyCollider;

    private BoxCollider coreCollider;
    private Animator animator;
    private bool opened;

    private void Start()
    {
        animator = GetComponent<Animator>();
        coreCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (inRange && Input.GetButtonDown("Interact"))
        {
            //if key hasn't been picked up and door is open
            if (keyCollider != null && opened)
            {
                keyCollider.PickUp();
                keyCollider = null;
                opened = !opened;
                return;
            }

            opened = !opened;
            animator.SetTrigger("Trigger");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            inRange = true;
            GameManager.instance.InteractPrompt();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            inRange = false;
            GameManager.instance.Hide();
        }
    }
}
