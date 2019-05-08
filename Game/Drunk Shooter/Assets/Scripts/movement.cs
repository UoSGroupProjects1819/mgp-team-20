using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    [SerializeField]
    public AudioClip[] soundEffects;

    [SerializeField]
    public Text healthDisplay;

    public GameObject player; //Used for shortening code - I can type "player" instead of GameObject.Find("player") etc
    public GameObject pistol;
    public GameObject flamethrower;
    public GameObject sniper;
    public GameObject autorifle;
    public GameObject shotgun;

    public float lastShot = -1.0f; //The time since the last shot, used for cooldowns. Default at -1.0f so the enemies don't aggro instantly
                                   //In the NPC file I will make it so they check if lastShot is higher than 0, meaning the player has shot, and then they can aggro
    public Vector3 playerPosition;
    public float health = 1000.0f;
    public float speed = 7.5f; //Movement speed of the player
    public float jumpSpeed = 7.5f; //Jump speed of the player
    public float gravity = 20.0f; //Gravity speed

    private float pitch = 0f; //Horizontal rotation; left and right
    private float yaw = 0f; //Vertical rotation; up and down
    private float maximumYaw = -75.0f; //Highest degree that the player can look up
    private float minimumYaw = 75.0f; //Lowest degree that the player can look down

    private float varianceX = 0.0f; //Variance for shotgun shells
    private float varianceY = 0.0f; //Variance for shotgun shells

    private float horizontalMovement = 0; //Horizontal movement set to 0 temporarily
    private float verticalMovement = 0; //Vertical movement set to 0 temporarily
    private Vector3 movementDirection = Vector3.zero; //Vector3.zero is the same as Vector3(0, 0, 0) - temporary

    Transform cameraTransform;

    private CharacterController controller;
    Vector3 position;
    Vector3 direction;

    int layerMask = 0; //Set the layer mask to 0 temporarily

    public GameObject reticleChild; //GameObject for the reticle to be set to

    public ParticleSystem onKillEffect; //Poof particle for enemy deaths

    void Start()
    {
        player = GameObject.Find("Player"); //Actually setting the GameObjects and Strings to not be blank
        pistol = GameObject.Find("banana"); //Has to be done in Start() or Awake(), can't be done anywhere else
        flamethrower = GameObject.Find("lamp");
        sniper = GameObject.Find("umbrella");
        autorifle = GameObject.Find("keytar");
        shotgun = GameObject.Find("trumpet");
        
        sniper.SetActive(false);

        layerMask = LayerMask.GetMask("NPC"); //Set the layer mask to "NPC" - only the NPCs will have this set
        reticleChild = this.gameObject.transform.GetChild(0).GetChild(24).gameObject; //Set the reticle to "reticleChild"
        //GetChild(0) is the Camera, and GetChild(0).GetChild(22) is the 23rd (including 0) game object set as a child of the Camera
        Cursor.lockState = CursorLockMode.Locked; //Don't let the cursor move
        Cursor.visible = false; //Make the cursor invisible - the cursor isn't necessary due to the automation of the reticle

        controller = GetComponent<CharacterController>();

        cameraTransform = Camera.main.transform; //Set the transformation of the main camera to this variable, quicker to do
        gameObject.transform.position = new Vector3(0, 0, 0); //Set the position of the player to 0, 0, 0
    }

    void Update()
    {
        playerPosition = gameObject.transform.position;
        healthDisplay.text = health.ToString();
        if (health <= 0)
            SceneManager.LoadScene("StartMenu");
        var heading = reticleChild.transform.position - cameraTransform.position; //Calculation of the angle from the Camera to the reticle
        var distance = heading.magnitude; //^
        var direction = heading / distance; //^
        if (player.transform.Find("Placeholder").gameObject.transform.childCount < 1) //If the "Placeholder" object (that's a child of "player") doesn't have anything in it
        {
            if (player.transform.Find("banana").gameObject.activeSelf == true)
            {
                pistol.transform.localPosition = new Vector3(1.0f, 0.2f, 1.0f); //Make sure it's in the right place
                pistol.transform.localRotation = Quaternion.Euler(60.0f, 0.0f, 105.0f); //in the right rotation
                pistol.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); //and is the right size
            }
            if (player.transform.Find("umbrella").gameObject.activeSelf == true)
            {
                sniper.transform.localPosition = new Vector3(1.0f, 0.2f, 0.1f);
                sniper.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                sniper.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
            //if (player.transform.Find("trumpet").gameObject.activeSelf == true)
            //{
            //    shotgun.transform.localPosition = new Vector3(1.0f, 0.2f, -0.5f);
            //    shotgun.transform.localRotation = Quaternion.Euler(0.0f, 5.0f, 0.0f);
            //    shotgun.transform.localScale = new Vector3(0.01f, 0.01f, 0.02f);
            //}
            //if (player.transform.Find("lamp"))
            //{
            //    flamethrower.transform.localPosition = new Vector3(1.0f, 0.2f, 1.0f);
            //    flamethrower.transform.localRotation = Quaternion.Euler(-10.0f, 0.0f, 0.0f);
            //    flamethrower.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            //}
            //if (player.transform.Find("keytar"))
            //{
            //    autorifle.transform.localPosition = new Vector3(1.0f, 0.2f, -0.5f);
            //    autorifle.transform.localRotation = Quaternion.Euler(0.0f, 5.0f, 0.0f);
            //    autorifle.transform.localScale = new Vector3(0.01f, 0.01f, 0.02f);
        }
            if (Input.GetMouseButtonDown(0)) //If you left click (not hold, just the click)
            {
                if (player.transform.Find("banana").gameObject.activeSelf == true) //If the player is holding a pistol they can only kill 1 enemy at a time
                {
                    if (Time.time - lastShot >= 0.5f)
                    {
                        AudioSource.PlayClipAtPoint(soundEffects[0], playerPosition);
                        Ray ray = Camera.main.ScreenPointToRay(reticleChild.transform.position); //Check a raycast through the reticle
                        RaycastHit[] hit = Physics.RaycastAll(ray.origin, direction, Mathf.Infinity, layerMask); //and save all the ones with the layer "NPC" to a list called "hit"
                        foreach (RaycastHit h in hit) //For all the things that are hit by the raycast
                        {
                            if (h.collider.tag == "Enemy") //if the raycast object has the tag "Enemy"
                            {
                                Debug.Log("HIT!"); //Send a message to the console saying "HIT!" - for testing purposes only, will not be seen by the player#
                                h.collider.gameObject.GetComponent<Animator>().SetTrigger("death");
                                h.collider.gameObject.GetComponent<NPC>().deathTimer = Time.time;
                                h.collider.gameObject.GetComponent<NPC>().deathActive = true;
                                break; //It has killed an enemy, so now stop killing enemies from that single shot - prevents collaterals
                            }
                        }
                    }
                        
                    lastShot = Time.time; //Set the timer for when you last shot to the current time - only here in case we want to put a cooldown on the pistol
                }
                if (player.transform.Find("umbrella").gameObject.activeSelf == true) //If the player is holding a sniper they can kill multiple enemies at a time (collaterals)
                {
                    if (Time.time - lastShot >= 1.5f) //If it has been at least 1.5 seconds since you last shot
                    {
                        AudioSource.PlayClipAtPoint(soundEffects[1], playerPosition);
                        Ray ray = Camera.main.ScreenPointToRay(reticleChild.transform.position); //Check a raycast through the reticle
                        RaycastHit[] hit = Physics.RaycastAll(ray.origin, direction, Mathf.Infinity, layerMask); //and save all the ones with the layer "NPC" to a list called "hit"
                        foreach (RaycastHit h in hit) //For all the things that are hit by the raycast
                        {
                            if (h.collider.tag == "Enemy") //if the raycast object has the tag "Enemy"
                            {
                                Debug.Log("HIT!"); //Send a message to the console saying "HIT!" - for testing purposes only, will not be seen by the player#
                                h.collider.gameObject.GetComponent<Animator>().SetTrigger("death");
                                h.collider.gameObject.GetComponent<NPC>().deathTimer = Time.time;
                                h.collider.gameObject.GetComponent<NPC>().deathActive = true;
                            }
                        }
                        lastShot = Time.time; //Set the timer for when you last shot to the current time
                    }
                }
                //if (player.transform.Find("trumpet").gameObject.activeSelf == true) //Variance doesn't seem to work, a variance of -0.5 to 0.5 is small at world center, massive elsewhere
                //{
                //    if (Time.time - lastShot >= 1.0f) //If it has been at least 1 second since you last shot
                //    {
                //        for (int i = 0; i >= 5; i++) //We want 6 bullets (0 to 5 inclusive), so loop the following code until that is met
                //        {
                //            varianceX = Random.Range(-0.1f, 0.1f); //Randomise the variance for each shell
                //            varianceY = Random.Range(-0.1f, 0.1f);
                //            Ray ray = Camera.main.ScreenPointToRay(reticleChild.transform.position + new Vector3(varianceX, varianceY, 0)); //Check a raycast through the reticle (with offset)
                //            RaycastHit[] hit = Physics.RaycastAll(ray.origin, direction, Mathf.Infinity, layerMask); //and save all the ones with the layer "NPC" to a list called "hit"
                //            foreach (RaycastHit h in hit) //For all the things that are hit by the raycast
                //            {
                //                if (h.collider.tag == "Enemy") //if the raycast object has the tag "Enemy"
                //                {
                //                    Debug.Log("HIT!"); //Send a message to the console saying "HIT!" - for testing purposes only, will not be seen by the player#
                //                    h.collider.gameObject.GetComponent<Animator>().SetTrigger("death");
                //                    h.collider.gameObject.GetComponent<NPC>().deathTimer = Time.time;
                //                    h.collider.gameObject.GetComponent<NPC>().deathActive = true;
                //                }
                //            }
                //        }
                //        lastShot = Time.time; //Set the timer for when you last shot to the current time
                //    }
                //}
            }
            if (Input.GetMouseButton(0)) //If you left click and hold
            {
                if (player.transform.Find("lamp")) //Flamethrower
                {
                }
                if (player.transform.Find("keytar")) //Autorifle - same as the pistol, except it's automatic
                {
                    if (Time.time - lastShot >= 0.5f)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(reticleChild.transform.position); //Check a raycast through the reticle
                        RaycastHit[] hit = Physics.RaycastAll(ray.origin, direction, Mathf.Infinity, layerMask); //and save all the ones with the layer "NPC" to a list called "hit"
                        foreach (RaycastHit h in hit) //For all the things that are hit by the raycast
                        {
                            if (h.collider.tag == "Enemy") //if the raycast object has the tag "Enemy"
                            {
                                Debug.Log("HIT!"); //Send a message to the console saying "HIT!" - for testing purposes only, will not be seen by the player#
                                h.collider.gameObject.GetComponent<Animator>().SetTrigger("death");
                                h.collider.gameObject.GetComponent<NPC>().deathTimer = Time.time;
                                h.collider.gameObject.GetComponent<NPC>().deathActive = true;
                                break; //The autorifle should only be able to kill 1 person per bullet, same as the pistol, although it shoots automatically (so the player doesn't have to spam)
                                //#MAY HAVE TO GIVE THE PISTOL A COOLDOWN IF THE PISTOL IS TOO OVERPOWERED - SOME PEOPLE CAN CLICK FASTER THAN 10 TIMES A SECOND SO THE PISTOL WOULD BE BETTER THAN THE AK
                                //#ALTHOUGH IN SAYING THAT, I DOUBT MANY PEOPLE CAN CLICK FASTER THAN 10 TIMES A SECOND WITHOUT BUMPING THE MOUSE OFF COURSE BY A LARGE MARGIN
                            }
                        }
                        lastShot = Time.time; //If the "Placeholder" object (that's a child of "player") has anything in it (E.G. picking up a stool)
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
        yaw = Mathf.Clamp(yaw, maximumYaw, minimumYaw); //Clamp the yaw between the maximum and minimum vertical axis, so the player can't "break their neck" or look upside down
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

    void OnControllerColliderHit(ControllerColliderHit hit) //Whilst the player is hitting *something*
    {
        Rigidbody attachedBody = hit.collider.attachedRigidbody; //Find the Rigidbody of the colliding item
        if (attachedBody == null || attachedBody.isKinematic) //If there is none, or it's kinematic
            return; //just stop
        if (hit.moveDirection.y < -0.3f)
            return;
        Vector3 collisionDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z); //Push the thing
        attachedBody.velocity = direction * speed; //Give it this speed
    }
}
