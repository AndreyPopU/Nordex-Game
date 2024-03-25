using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private float movementSpeed = 5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * movementSpeed * Time.deltaTime;

        transform.Translate(movement);
    }
}