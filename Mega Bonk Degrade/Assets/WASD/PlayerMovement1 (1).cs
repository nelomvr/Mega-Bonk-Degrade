using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;      // Max speed
    public float Speed = 10f;         // Force multiplier
    public float groundDrag = 5f;
    public float airMultiplier = 0.5f; // Less control in air

    [Header("GroundCheck")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true; //jumping at start
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        GroundCheck();
        ApplyDrag();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //trigger on key press, not hold
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * Speed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * Speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void GroundCheck()
    {
        // ground detection 
        float checkRadius = 0.3f;
        grounded = Physics.CheckSphere(transform.position + Vector3.down * (playerHeight / 2), checkRadius, whatIsGround);

        // Debug line 
        Debug.Log("Grounded: " + grounded);
    }

    private void ApplyDrag()
    {
        rb.drag = grounded ? groundDrag : 0f;
    }

    private void SpeedControl()
    {
        //horizontal velocity
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
