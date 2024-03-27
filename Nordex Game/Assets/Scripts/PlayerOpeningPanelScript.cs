using UnityEngine;

public class PlayerOpeningPanelScript : MonoBehaviour
{
    private KeyCode openKey = KeyCode.E;

    private float moveDistance = 2f;

    private bool isInTriggerZone = false;
    private bool hasMovedPanel = false;

    private Transform panelTransform;

    private BoxCollider panelCollider;

    private GameObject firstPersonCamera;
    private GameObject puzzleCamera;

    void Start()
    {
        firstPersonCamera = GameObject.FindWithTag("FirstPersonCamera");
        puzzleCamera = GameObject.FindWithTag("PuzzleCamera");

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
            panelCollider = other.GetComponent<BoxCollider>(); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Panel"))
        {
            isInTriggerZone = false;

            panelTransform = null;
            panelCollider = null; 
        }
    }

    void OpenPanel()
    {
        panelCollider.enabled = false; 

        Rigidbody playerRigidbody = GetComponent<Rigidbody>(); 
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        firstPersonCamera.SetActive(false);
        puzzleCamera.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        transform.position += new Vector3(0f, 0f, -3f);
    }
}