using UnityEngine;

public class CabinetDoor : MonoBehaviour
{
    public bool opened;
    public bool inrange;
    public Animator animator;
    public BoxCollider corecollider;
    public BoxCollider keycollider;
    private void Start()
    {
        animator = GetComponent<Animator>();
        corecollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (inrange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (keycollider.enabled && !opened)
                {
                    corecollider.enabled = true;
                    keycollider.enabled = false;
                    return;
                }

                animator.SetBool("Open", opened);
                animator.SetTrigger("Trigger");

                //if doesn't have key and closed
                if (!keycollider.enabled && !opened)
                {
                    corecollider.enabled = false;
                    keycollider.enabled = true;
                    return;
                }
                opened = !opened;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
            inrange = false;
    }
}
