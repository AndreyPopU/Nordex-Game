using UnityEngine;

public class Puzzle2StarterScript : MonoBehaviour
{
    public GameObject puzzle2;
    public GameObject puzzle2FAKE;
    public GameObject player;
    public GameObject restartButton;

    public Transform puzzleCenter;
    public Transform cameraTargetPosition;

    public Collider playerCollider;

    public float cameraTransitionSpeed = 2.0f;

    public Camera mainCamera;

    private bool isPlayerInArea = false;
    private bool isPuzzleActive = false;

    private Puzzle2StarterScript puzzle2StarterScript;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private Vector3 storedCameraPosition;
    private Quaternion storedCameraRotation;
    private Vector3 storedPlayerPosition;

    private RigidbodyConstraints originalConstraints;
    private Player playerScript;
    private Rigidbody playerRigidbody;

    void Start()
    {
        puzzle2StarterScript = GetComponent<Puzzle2StarterScript>();

        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;
        }

        playerScript = player.GetComponent<Player>();

        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                originalConstraints = playerRigidbody.constraints;
            }
        }
    }

    void Update()
    {
        if (isPlayerInArea && Input.GetKeyUp(KeyCode.E) && !isPuzzleActive)
        {
            // Store the current positions before starting the puzzle
            storedCameraPosition = mainCamera.transform.position;
            storedCameraRotation = mainCamera.transform.rotation;
            storedPlayerPosition = player.transform.position;

            StartPuzzle();
            puzzle2FAKE.SetActive(false);
        }

        if (isPuzzleActive && mainCamera != null)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraTargetPosition.position, Time.deltaTime * cameraTransitionSpeed);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraTargetPosition.rotation, Time.deltaTime * cameraTransitionSpeed);
        }
    }

    private void StartPuzzle()
    {
        if (puzzleCenter != null && mainCamera != null)
        {
            // Move and rotate the camera
            StartCoroutine(SmoothTransitionCamera(mainCamera.transform.position, mainCamera.transform.rotation, cameraTargetPosition.position, cameraTargetPosition.rotation));
        }

        if (puzzle2 != null)
        {
            puzzle2.SetActive(true);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        // Disable player controls
        if (playerScript != null)
        {
            playerScript.focused = true;
        }

        // Disable mouse look and make cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze player position
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        playerCollider.enabled = false;

        isPuzzleActive = true;
    }

    private System.Collections.IEnumerator SmoothTransitionCamera(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot)
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = endPos;
        mainCamera.transform.rotation = endRot;
    }

    private System.Collections.IEnumerator SmoothTransitionPlayer(Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            player.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = endPos;
    }

    public void ResetCameraAndPlayer()
    {
        if (mainCamera != null)
        {
            StartCoroutine(SmoothTransitionCamera(mainCamera.transform.position, mainCamera.transform.rotation, storedCameraPosition, storedCameraRotation));
        }

        if (player != null)
        {
            StartCoroutine(SmoothTransitionPlayer(player.transform.position, storedPlayerPosition));
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = originalConstraints;
        }

        if (playerScript != null)
        {
            playerScript.focused = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPuzzleActive = false;

        if (puzzle2 != null)
        {
            puzzle2.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }

        if (puzzle2FAKE != null)
        {
            puzzle2FAKE.SetActive(true);
        }

        playerCollider.enabled = true;

        puzzle2StarterScript.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInArea = false;
        }
    }
}