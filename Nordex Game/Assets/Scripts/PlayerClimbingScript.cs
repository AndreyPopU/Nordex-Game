using UnityEngine;

public class PlayerClimbingScript : MonoBehaviour
{
    private float climbSpeed = 3f; 
    private float fallSpeed = 4f; 

    private bool isClimbing = false; 

    private Transform ladderTransform; 

    private Vector3 initialPosition; 

    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            ladderTransform = other.transform;
            initialPosition = transform.position;
            rb.useGravity = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.useGravity = true; 
        }
    }

    void Update()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbDirection = ladderTransform.up * verticalInput * climbSpeed * Time.deltaTime;

            // Move the player up/down the ladder
            transform.position += climbDirection;

            if (verticalInput == 0f)
            {
                rb.AddForce(-ladderTransform.up * fallSpeed * Time.deltaTime, ForceMode.Acceleration);
            }
        }
    }
}