using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    private bool readyToJump;

    public float walkSpeed;
    public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evitar que la gravedad afecte la rotación
        readyToJump = true; // Asegúrate de que esté lista para saltar
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
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * (grounded ? 10 : 10 * airMultiplier), ForceMode.Acceleration);
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
}