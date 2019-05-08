using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Quaternion rotation;
    public Vector3 position;

    public GameObject player;
    public GameObject banana;
    public GameObject umbrella;

    void Start()
    {
        player = GameObject.Find("Player");
        banana = player.GetComponent<movement>().pistol;
        umbrella = player.GetComponent<movement>().sniper;
    }
    
    void Update()
    {
        if ((Vector3.Distance(player.GetComponent<movement>().playerPosition, transform.position)) <= 2)
        {
             if (gameObject.name == "bananaSpawner")
             {
                if (banana.activeSelf == false)
                {
                    banana.SetActive(true);
                    umbrella.SetActive(false);
                }
             }
             if (gameObject.name == "umbrellaSpawner")
             {
                if (umbrella.activeSelf == false)
                {
                    banana.SetActive(false);
                    umbrella.SetActive(true);
                }
            }
        }
        rotation = gameObject.transform.rotation;
        position = gameObject.transform.position;
        gameObject.transform.Rotate(0, 50 * Time.deltaTime, 0);
        gameObject.transform.position = new Vector3(position.x, Mathf.Sin(Time.time) /2 + 2, position.z);
    }
}
