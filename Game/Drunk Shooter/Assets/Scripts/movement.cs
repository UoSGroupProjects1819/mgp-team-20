using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 7.5f; //Movement speed of the player
    public float jumpSpeed = 12.5f; //Jump speed of the player
    public float gravity = 20.0f; //Gravity speed

    private float pitch = 0f; //Horizontal rotation; left and right
    private float yaw = 0f; //Vertical rotation; up and down
    private float minYaw = -70.0f; //Lowest degree that the player can look down
    private float maxYaw = 55.0f; //Highest degree that the player can look up

    private float horizontalMovement = 0; //Horizontal movement set to 0 temporarily
    private float verticalMovement = 0; //Vertical movement set to 0 temporarily
    private Vector3 movementDirection = Vector3.zero; //Vector3.zero is the same as Vector3(0, 0, 0) - temporary

    Transform cameraTransform;

    private CharacterController controller;
    Vector3 position;
    Vector3 direction;

    int layerMask = 0; //Set the layer mask to 0 temporarily

    public GameObject reticleChild; //GameObject for the reticle to be set to

    void Start()
    {
        layerMask = LayerMask.GetMask("NPC"); //Set the layer mask to "NPC" - only the NPCs will have this set
        reticleChild = this.gameObject.transform.GetChild(0).GetChild(22).gameObject; //Set the reticle to "reticleChild"
        //GetChild(0) is the Camera, and GetChild(0).GetChild(22) is the 23rd (including 0) game object set as a child of the Camera
        Cursor.lockState = CursorLockMode.Locked; //Don't let the cursor move
        Cursor.visible = false; //Make the cursor invisible - the cursor isn't necessary due to the automation of the reticle

        controller = GetComponent<CharacterController>();

        cameraTransform = Camera.main.transform; //Set the transformation of the main camera to this variable, quicker to do
        gameObject.transform.position = new Vector3(0, 0, 0); //Set the position of the player to 0, 0, 0
    }

    void Update()
    {
        var heading = reticleChild.transform.position - cameraTransform.position; //Calculation of the angle from the Camera to the reticle
        var distance = heading.magnitude; //^
        var direction = heading / distance; //^
        if (Input.GetMouseButtonDown(0)) //If you left click (not hold, just the click)
        {
            Ray ray = Camera.main.ScreenPointToRay(reticleChild.transform.position); //Check a raycast through the reticle
            RaycastHit[] hit = Physics.RaycastAll(ray.origin, direction, Mathf.Infinity, layerMask); //and save all the ones with the layer "NPC" to a list called "hit"
            foreach (RaycastHit h in hit) //For all the things that are hit by the raycast
            {
                if (h.collider.tag == "Enemy") //if the raycast object has the tag "Enemy"
                {
                    Debug.Log("HIT!"); //Send a message to the console saying "HIT!" - for testing purposes only, will not be seen by the player
                    h.collider.gameObject.SetActive(false); //Disable the object - Destroying the object is too slow of a process to do
                }
            }
        }
    }

    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis("Mouse X") * speed; //Horizontal movement of the mouse
        verticalMovement = Input.GetAxis("Mouse Y") * speed; //Vertical movement of the mouse
        pitch += horizontalMovement; //Add the mouse's horizontal movement to the pitch
        yaw += -verticalMovement; //Add the negative of the mouse's movement to the yaw (because of how the mouse works)
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw); //Clamp the yaw between the maximum and minimum vertical axis, so the player can't "break their neck" or look upside down
        transform.eulerAngles = new Vector3(yaw, pitch, 0f); //Set the angle that the player is looking to that

        if (controller.isGrounded) //If the player is grounded
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")); //Set movement direction to the Vector3 of their mouse movement
            movementDirection = transform.TransformDirection(movementDirection); //Make them face that direction
            movementDirection = movementDirection * speed; //and then multiply it by a factor of "speed"

            if (Input.GetButton("Jump")) //If they click jump (remember - only if they're grounded)
            {
                movementDirection.y = jumpSpeed; //Then set their jump direction to the jumpSpeed
            }
        }

        movementDirection.y -= (gravity * Time.deltaTime); //Make them fall over time
        controller.Move(movementDirection * Time.deltaTime); //Actually move the player
    }
}
