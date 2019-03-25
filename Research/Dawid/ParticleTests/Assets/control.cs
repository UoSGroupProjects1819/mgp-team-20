using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    public GameObject ProjToTest;
    public ParticleSystem ProjPS;
    public ParticleSystem ProjHit;

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            ProjToTest.SetActive(true);
        }

        if(ProjPS.IsAlive() == false)
        {
            if (ProjHit.IsAlive() == false)
            {
                ProjToTest.SetActive(false);
                ProjToTest.transform.position = new Vector3(0, 0, 0);
            }
        }

    }

}