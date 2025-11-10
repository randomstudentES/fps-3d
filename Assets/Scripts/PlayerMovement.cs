using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    private bool readyToJump;

    public float walkSpeed;
    public float sprintSpeed;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftControl;
    public KeyCode crouchKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Manejo de rampas")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private enum MovementState
    {
        walking, sprinting, air, crouching
    }

    private MovementState state;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evitar que la gravedad afecte la rotación
        readyToJump = true; // Asegúrate de que esté lista para saltar

        startYScale = transform.localScale.y;

    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (playerHeight * 0.5f); // Ajustando origen del raycast
        float rayDistance = playerHeight * 0.5f + 0.2f;

        grounded = Physics.Raycast(rayOrigin, Vector3.down, rayDistance, whatIsGround);
        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red);  // Visualiza el raycast

        myInput();
        stateHandler();

        rb.drag = grounded ? groundDrag : 0;
    }

    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded) // Cambien a GetKeyDown
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(resetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    public void stateHandler()
    {
        
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        } else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        } else
        {
            state = MovementState.air;
        }
    }
    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * (grounded ? 10 : 10 * airMultiplier), ForceMode.Acceleration);

        rb.useGravity = !OnSlope();
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        // Dirección hacia abajo
        Vector3 rayDirection = Vector3.down;
        float rayDistance = playerHeight * 0.5f;

        // Dibuja el rayo en la escena
        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

        if (Physics.Raycast(transform.position, rayDirection, out slopeHit, rayDistance))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }


    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}