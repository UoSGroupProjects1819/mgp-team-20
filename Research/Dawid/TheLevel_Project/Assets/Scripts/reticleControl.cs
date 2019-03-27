using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticleControl : MonoBehaviour {

    [SerializeField] //Allows me to add the waypoints in the Inspector for Unity rather than programming it in
    Transform[] waypoints; //Make a list for waypoints

    [SerializeField]
    float reticleMovementSpeed = 2.5f; //Set the movement speed of the reticle (if changed in the Inspector, it overwrites it here)

    int waypointIndex = 0; //Temporarily set the integer to 0 to hold the amount of waypoints there are

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position; //Set the position of the reticle to the position of the first waypoint
    }

    void Update()
    {
        MoveReticle(); //Go to the MoveRecticle() function
    }

    void MoveReticle()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, reticleMovementSpeed * Time.deltaTime); //Set the position of the
        //reticle to move towards the next waypoint over time
        if (transform.position == waypoints[waypointIndex].transform.position) //If it's at the waypoint, go to the next one
            waypointIndex += 1;
        if (waypointIndex == waypoints.Length) //If it's at the end of the waypoint list, restart the list
            waypointIndex = 0;
    }
}
