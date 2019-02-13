﻿using System.Collections;

using System.Collections.Generic;

using UnityEngine;


public class ReticleControl : MonoBehaviour {

	[SerializeField]
	Transform[] waypoints;

	[SerializeField]
	float movementSpeed = 5f;

	int waypointIndex = 0;

	void Start() 
	{
		transform.position = waypoints[waypointIndex].transform.position;
	}

	void Update()
	{
		Move();
	}

	void Move()
	{
		transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movementSpeed * Time.deltaTime);
		if (transform.position == waypoints[waypointIndex].transform.position)
			waypointIndex += 1;
		if (waypointIndex == waypoints.Length)
			waypointIndex = 0;
	}
}
