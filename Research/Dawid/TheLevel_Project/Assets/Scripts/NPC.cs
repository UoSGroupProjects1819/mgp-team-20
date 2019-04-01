using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    //public Vector3[] spawnLocations = { new Vector3(-4.5f, -0.9f, 2.9f),
    //new Vector3(3.7f, -0.9f, 2.9f),
    //new Vector3(-0.5f, -0.9f, -0.6f),
    //new Vector3(4.7f, -0.9f, -4.5f),
    //new Vector3(-4.2f, -0.9f, -8.6f),
    //new Vector3(0.2f, -0.9f, -10.0f) };

    public GameObject player;

    public float deathTimer;
    public bool deathActive;
    public ParticleSystem onKillEffect;
    public float aggro;

    private float movementSpeed = 1.0f;
    private float attackCooldown;
    private float lastShotNPC = -1.0f;

    private float hitChance = 5;
    private float random = 0;
    private int spawnLocationChoice;

    void Start()
    {
        player = GameObject.Find("Player");
        aggro = player.GetComponent<Movement>().lastShot;
    }

    void KillEnemy()
    {
        Instantiate(onKillEffect, gameObject.transform.position, Quaternion.identity);
        switch (spawnLocationChoice)
        {
            case 0:
                gameObject.transform.position = new Vector3(-4.5f, -0.9f, 2.9f);
                break;
            case 1:
                gameObject.transform.position = new Vector3(3.7f, -0.9f, 2.9f);
                break;
            case 2:
                gameObject.transform.position = new Vector3(-0.5f, -0.9f, -0.6f);
                break;
            case 3:
                gameObject.transform.position = new Vector3(4.7f, -0.9f, -8.6f);
                break;
            case 4:
                gameObject.transform.position = new Vector3(-4.2f, -0.9f, -8.6f);
                break;
            case 5:
                gameObject.transform.position = new Vector3(0.2f, -0.9f, -10.0f);
                break;
        }
        deathActive = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        spawnLocationChoice = Random.Range(0, 5);
        if (deathActive && deathTimer > 0)
            deathTimer -= Time.deltaTime;
        if (deathActive && deathTimer <= 0)
            KillEnemy();
        if (aggro < 0.0f)
            lastShotNPC = Time.time;
    }

    void LateUpdate() //Update after all the Update() functions have done, hence "LateUpdate()"
    {
        Vector3 cameraPos = Camera.main.transform.position; //Save the camera's position to the variable "cameraPos"
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis (unnecessary for Y axis, and will look weird)
        transform.LookAt(cameraPos); //Look at the Camera
        if (aggro > 0.0f)
        {
            //Aggro onto the player only after they have taken their first shot of the game
            attackCooldown = Random.Range(2, 4);
            if (Time.time - lastShotNPC >= attackCooldown)
            {
                //Shoot the player
                random = Random.Range(1, 10); // 4/10 hit chance
                if (random > hitChance)
                    player.GetComponent<Movement>().health -= 1.0f;
            }
        }
    }
}
