using System.Collections;
using UnityEngine;

public class PlayerLeavingScript : MonoBehaviour
{
    private PlayerLeavingScript playerLeavingScript;
    private Player playerScript;

    public Tablet tabletScript;

    public Canvas analyticsCanvas;

    public AudioSource truckLeavingSound;

    private bool isPlayerInTrigger = false;

    void Start()
    {
        playerLeavingScript = GetComponent<PlayerLeavingScript>();
        playerScript = GetComponent<Player>();
        
        playerLeavingScript.enabled = false;
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!analyticsCanvas.enabled)
            {
                analyticsCanvas.enabled = true;
            }

            if (!truckLeavingSound.isPlaying)
            {
                StartCoroutine(PlayTruckLeavingSound());
            }
            
            if (playerScript.enabled && tabletScript.enabled)
            {
                playerScript.enabled = false;
                tabletScript.enabled = false;
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "pickup_truck_unwrapped")
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "pickup_truck_unwrapped")
        {
            isPlayerInTrigger = false;
        }
    }

    private IEnumerator PlayTruckLeavingSound()
    {
        truckLeavingSound.Play();
        yield return new WaitForSeconds(truckLeavingSound.clip.length);
        truckLeavingSound.enabled = false;
    }
}