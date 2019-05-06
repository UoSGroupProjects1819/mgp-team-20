using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectileEffect;
    public GameObject onHitEffect;
    public Vector3 playerLocation;
    public Vector3 moveDirection;

    private void Start()
    {
        playerLocation = GameObject.Find("Player").transform.position;
        gameObject.transform.LookAt(playerLocation);
        projectileEffect = gameObject.transform.GetChild(0).gameObject;
        onHitEffect = gameObject.transform.GetChild(1).gameObject;

        Physics.IgnoreLayerCollision(10, 11);
        
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        moveDirection = (playerLocation - gameObject.transform.position).normalized * 20.0f;
        gameObject.GetComponent<Rigidbody>().velocity = moveDirection;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "Main Camera")
            GameObject.Find("Player").GetComponent<movement>().health -= 1.0f;
        projectileEffect.SetActive(false);
        onHitEffect.SetActive(true);
        Destroy(gameObject, 1f);
    }
}
