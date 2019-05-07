using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevival : MonoBehaviour
{
    public List<GameObject> objects;
    public List<GameObject> enemies;

    void Start()
    {
        StartCoroutine(LateStart());
    }

    void Update()
    {
        try
        {
            foreach (GameObject x in objects)
                if (x.activeSelf == false)
                    StartCoroutine(Revive(x));
        }
        catch
        {
        }
    }

    public IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(1);
        foreach (GameObject x in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            if (x.name.Contains("Enemy"))
                objects.Add(x);
    }

    public IEnumerator Revive(GameObject target)
    {
        yield return new WaitForSecondsRealtime(10);
        target.SetActive(true);
    }
}
