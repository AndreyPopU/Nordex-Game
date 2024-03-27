using UnityEngine;

public class MouseScript : MonoBehaviour
{
    void Start()
    {
        // Lock the cursor to the game window center
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        Cursor.visible = false;
    }
}