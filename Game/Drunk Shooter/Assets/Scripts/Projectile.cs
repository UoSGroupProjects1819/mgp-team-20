using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileEffect;
    public GameObject onHitEffect;

    private void Start()
    {
        projectileEffect = gameObject.transform.GetChild(0).gameObject;
        onHitEffect = gameObject.transform.GetChild(1).gameObject;

        Physics.IgnoreLayerCollision(10, 11);

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
