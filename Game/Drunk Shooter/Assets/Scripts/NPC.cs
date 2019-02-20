using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    
	void LateUpdate () {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.y = transform.position.y; //Look at Camera on all axis except Y axis
        transform.LookAt(cameraPos);
	}
}
