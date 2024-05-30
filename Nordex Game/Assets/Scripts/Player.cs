using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public bool grounded;
    public float Jumpforce;
    public bool jumping;

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public bool haskey;

    //Other
    [HideInInspector] public CapsuleCollider coreCollider;
    [HideInInspector] public Rigidbody rb;

    // Puzzles
    public bool hasToolbox;
    public bool focused;
    public Puzzle puzzleInRange;
    public bool turbineSpin;
    #region Movement

    private float baseSpeed;
    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Input
    float x, y;

    #endregion

    //check players gender and add sound
    public AudioSource source;
    public AudioClip clipW, clipM;
    public bool canMove;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        coreCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        baseSpeed = moveSpeed;
    }

    void Start()
    {
        //set source
        source = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Movement();

        Jump();
    }

    private void Update()
    {
        if (!canMove) return;

        if (Input.GetButtonDown("Interact")) FocusPuzzle();

        if (focused) return;

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        Look();

        if (Input.GetKeyDown(KeyCode.T)) transform.position = new Vector3(0, 2, -2);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            grounded = false;
            jumping = true;
        }
    }

     public void Jump()
     {
        if (jumping)
        {
            grounded = false;
            rb.AddForce(Vector3.up * Jumpforce * Time.fixedDeltaTime, ForceMode.Impulse);
            jumping = false;
        }
    }

    public void FocusPuzzle()
    {
        if (puzzleInRange != null && puzzleInRange.finishedFocusing)
        {
            if (!focused) puzzleInRange.Focus(puzzleInRange.focusTransform);
            else puzzleInRange.Focus(playerCam.transform);
        }
    }

    private void Movement()
    {
        if (focused) return;

        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        if (!grounded) moveSpeed = baseSpeed / 8;
        else moveSpeed = baseSpeed;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }


    private float desiredX;
    private void Look()
    {
        if (focused) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        if (collision.gameObject.tag == "Bounds" && !source.isPlaying)
        {
            if (GameManager.instance.man) source.clip = clipM;
            else source.clip = clipW;
            source.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Puzzle puzzle)) puzzleInRange = puzzle;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Puzzle puzzle) && puzzleInRange == puzzle) puzzleInRange = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - Vector3.up * .85f, .2f);
    }
}

