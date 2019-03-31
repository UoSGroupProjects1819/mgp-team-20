using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public GameObject bullet;
    public GameObject player;

    public float deathTimer;
    public bool deathActive;
    public ParticleSystem onKillEffect;
    public float aggro;

    private float movementSpeed = 1.0f;
    private bool bulletTravel = false;
    private float attackCooldown;
    private float lastShotNPC = -1.0f;

    public void Start()
    {
        player = GameObject.Find("Player");
        aggro = player.GetComponent<Movement>().lastShot;
    }

    public void KillEnemy()
    {
        Instantiate(onKillEffect, gameObject.transform.position, Quaternion.identity);
        deathActive = false;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (deathActive && deathTimer > 0)
            deathTimer -= Time.deltaTime;
        if (deathActive && deathTimer <= 0)
            KillEnemy();
        if (aggro < 0.0f)
            lastShotNPC = Time.time;
        if (bulletTravel)
            bullet.transform.position += transform.forward * Time.time * movementSpeed;
    }

    public void LateUpdate () //Update after all the Update() functions have done, hence "LateUpdate()"
    {
        Vector3 cameraPos = Camera.main.transform.position; //Save the camera's position to the variable "cameraPos"
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis (unnecessary for Y axis, and will look weird)
        transform.LookAt(cameraPos); //Look at the Camera
        //if (aggro > 0.0f)
        //{
            //Aggro onto the player only after they have taken their first shot of the game
            attackCooldown = Random.Range(1, 3);
            if (Input.GetMouseButtonDown(0))
            {
            //Shoot the player
                GameObject newProj = Instantiate(bullet, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                newProj.GetComponent<Rigidbody>().AddForce(player.transform.position * 40f);
               // newProj.transform.LookAt(player.transform);
            }
        //}
    }
}
