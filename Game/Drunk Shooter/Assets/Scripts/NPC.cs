using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public GameObject bullet;
    public GameObject player;
    public Vector3 cameraPos;
    public int hitChance = 6;
    public int randomiser;
    public float revivalTimer;
    public float deathTimer;
    public bool deathActive;
    public ParticleSystem onKillEffect;
    public float aggro;
    public Vector3 heldPlayerLocation;
    public float attackCooldown;

    private float shootTime = -1.0f;
    private float movementSpeed = 1.0f;

    public void Start()
    {
        player = GameObject.Find("Player");
    }

    public void KillEnemy()
    {
        revivalTimer = 10;
        Instantiate(onKillEffect, gameObject.transform.position, Quaternion.identity);
        deathActive = false;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        try
        {
            aggro = player.GetComponent<movement>().lastShot;
        }
        catch
        {
            aggro = -1;
        }
        if (deathActive && deathTimer > 0)
            deathTimer -= Time.time;
        if (deathActive && deathTimer <= 0)
            KillEnemy();
    }

    public void LateUpdate () //Update after all the Update() functions have done, hence "LateUpdate()"
    {
        cameraPos = Camera.main.transform.position; //Save the camera's position to the variable "cameraPos"
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis (unnecessary for Y axis, and will look weird)
        transform.LookAt(cameraPos); //Look at the Camera
        if (aggro >= 0.0f)
        {
            //Aggro onto the player only after they have taken their first shot of the game
            attackCooldown = Random.Range(2, 4);
            if (Time.time - shootTime >= attackCooldown)
            {
                heldPlayerLocation = (cameraPos - new Vector3(0.0f, 1.0f, 0.0f));
                GameObject newProj = Instantiate(bullet, gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity) as GameObject;
                newProj.transform.LookAt(heldPlayerLocation);
                shootTime = Time.time;
            }
            //Shoot the player
            // newProj.transform.LookAt(player.transform);
        }
    }
}