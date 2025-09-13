using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirdperson : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform Player;
    public Transform PlayerObject;
    public Rigidbody rb;

    public float RotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = Player.position - new Vector3(transform.position.x, Player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            PlayerObject.forward = Vector3.Slerp(PlayerObject.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);


    }
}
