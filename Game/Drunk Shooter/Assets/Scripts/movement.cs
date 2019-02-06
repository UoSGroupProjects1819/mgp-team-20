using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 10.0f;
    public float gravity = 20.0f;

    private float pitch = 0f; //Horizontal rotation; left and right
    private float yaw = 0f; //Vertical rotation; up and down
    private float minYaw = -35.0f;
    private float maxYaw = 45.0f;

    private float horizontalMovement = 0;
    private float verticalMovement = 0;
    private Vector3 movementDirection = Vector3.zero;

    Transform cameraTransform;


    private CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();

        cameraTransform = Camera.main.transform;
        gameObject.transform.position = new Vector3(0, 0, 0);
    }
    
    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Mouse X") * speed;
        verticalMovement = Input.GetAxis("Mouse Y") * speed;
        pitch += horizontalMovement;
        yaw += -verticalMovement;
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        transform.eulerAngles = new Vector3(yaw, pitch, 0f);

        if (controller.isGrounded) //If the player is grounded
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection = movementDirection * speed;

            if (Input.GetButton("Jump"))
            {
                movementDirection.y = jumpSpeed;
            }
        }

        movementDirection.y -= (gravity * Time.deltaTime);
        controller.Move(movementDirection * Time.deltaTime);
    }
}
