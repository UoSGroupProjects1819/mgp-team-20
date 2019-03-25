using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject target;
    public GameObject Projectile;
    public ParticleSystem OnHitEffect;

    Vector3 origin = new Vector3(0,0,0); 


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newProj = Instantiate(Projectile, transform.position, Quaternion.LookRotation(transform.position - target.transform.position * -1, Vector3.up)) as GameObject;
            newProj.GetComponent<Rigidbody>().AddForce(target.transform.position * 40f);
        }

    }
}
