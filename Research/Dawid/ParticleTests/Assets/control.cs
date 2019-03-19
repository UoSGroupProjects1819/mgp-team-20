using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    public GameObject particle;
    void Update()
    {
        var mousePos = Input.mousePosition;

        if (Input.GetButtonDown("Fire1"))
        {
            particle.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

    }

}