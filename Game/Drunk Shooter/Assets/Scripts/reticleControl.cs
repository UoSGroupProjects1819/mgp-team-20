using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticleControl : MonoBehaviour {

    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    float reticleMovementSpeed = 2.5f;

    int waypointIndex = 0;

    void Start()
    {
        
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update()
    {
        MoveReticle();
    }

    void MoveReticle()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, reticleMovementSpeed * Time.deltaTime);
        if (transform.position == waypoints[waypointIndex].transform.position)
            waypointIndex += 1;
        if (waypointIndex == waypoints.Length)
            waypointIndex = 0;
    }
}
