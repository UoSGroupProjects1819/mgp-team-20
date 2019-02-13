using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class FaceCamera : MonoBehaviour
{
	void LateUpdate()
	{
		Vector3 cameraPos = Camera.main.transform.position;
		cameraPos.y = transform.position.y;
		transform.LookAt(cameraPos);
	}
}
