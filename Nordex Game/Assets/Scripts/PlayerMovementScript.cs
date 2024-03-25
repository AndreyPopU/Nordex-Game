using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody rb;

    private float movementSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Store the current vertical velocity
        float verticalVelocity = rb.velocity.y;

        // Calculate the horizontal movement input
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveInput = transform.TransformDirection(moveInput);

        // Apply horizontal movement
        rb.velocity = new Vector3(moveInput.x * movementSpeed, verticalVelocity, moveInput.z * movementSpeed);
    }
}