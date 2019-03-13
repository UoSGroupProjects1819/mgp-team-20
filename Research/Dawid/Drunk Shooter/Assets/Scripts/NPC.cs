using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    
	void LateUpdate () //Update after all the Update() functions have done, hence "LateUpdate()"
    {
        Vector3 cameraPos = Camera.main.transform.position; //Save the camera's position to the variable "cameraPos"
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis (unnecessary for Y axis, and will look weird)
        transform.LookAt(cameraPos); //Look at the Camera
	}
}
