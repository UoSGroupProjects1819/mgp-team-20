using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileEffect;
    public GameObject onHitEffect;

    private void Start()
    {
        Debug.Log("Projectile here!");
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        projectileEffect.SetActive(false);
        onHitEffect.SetActive(true);
        Destroy(gameObject, 1f);
    }
}
