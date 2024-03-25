using UnityEngine;

public class FirstPersonCameraScript : MonoBehaviour
{
    private float mouseSensitivity = 2f; 
    private float verticalRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.parent.Rotate(Vector3.up * mouseX); 

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Limit vertical rotation to avoid flipping
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}