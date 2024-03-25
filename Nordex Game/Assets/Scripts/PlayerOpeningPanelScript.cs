using UnityEngine;

public class PlayerOpeningPanelScript : MonoBehaviour
{
    private KeyCode openKey = KeyCode.E;

    private float moveDistance = 2f;

    private bool isInTriggerZone = false;
    private bool hasMovedPanel = false;

    private Transform panelTransform;

    private GameObject player;
    private GameObject firstPersonCamera;
    private GameObject puzzleCamera;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        firstPersonCamera = GameObject.FindWithTag("FirstPersonCamera");
        puzzleCamera = GameObject.FindWithTag("PuzzleCamera");

        if (player == null)
        {
            Debug.LogError("Player game object not found.");
        }

        if (firstPersonCamera == null)
        {
            Debug.LogError("First person camera game object not found.");
        }

        if (puzzleCamera == null)
        {
            Debug.LogError("Puzzle camera game object not found.");
        }

        else
        {
            puzzleCamera.SetActive(false);
        }
    }

    void Update()
    {
        if (isInTriggerZone && Input.GetKeyDown(openKey) && !hasMovedPanel)
        {
            if (panelTransform != null)
            {
                panelTransform.Translate(-panelTransform.forward * moveDistance);

                hasMovedPanel = true;

                OpenPanel();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Panel"))
        {
            isInTriggerZone = true;

            panelTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Panel"))
        {
            isInTriggerZone = false;

            panelTransform = null;
        }
    }

    void OpenPanel()
    {
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.velocity = Vector3.zero; 
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll; 

        firstPersonCamera.SetActive(false);
        puzzleCamera.SetActive(true);
    }
}