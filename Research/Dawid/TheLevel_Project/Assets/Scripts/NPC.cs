using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public float deathTimer;
    public bool deathActive;
    public ParticleSystem onKillEffect;

    public void KillEnemy()
    {
        Instantiate(onKillEffect, gameObject.transform.position, Quaternion.identity);
        gameObject.SetActive(false); //Disable the object - Destroying the object is too slow of a process to do

    }

    private void Update()
    {
        if (deathActive && deathTimer > 0)
        {
            deathTimer -= Time.deltaTime;
        }

        if(deathActive && deathTimer <= 0)
        {
            KillEnemy();
        }

    }

    void LateUpdate () //Update after all the Update() functions have done, hence "LateUpdate()"
    {
        Vector3 cameraPos = Camera.main.transform.position; //Save the camera's position to the variable "cameraPos"
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis (unnecessary for Y axis, and will look weird)
        transform.LookAt(cameraPos); //Look at the Camera
	}
}
