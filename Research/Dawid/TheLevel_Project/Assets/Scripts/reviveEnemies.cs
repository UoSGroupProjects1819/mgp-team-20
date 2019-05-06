using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reviveEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> deadEnemies;
    private List<GameObject> temporaryList;

    void Start()
    {
        GameObject[] temporaryList = Resources.FindObjectsOfTypeAll<GameObject>(); //Temporary list to store all GO's
        foreach (GameObject x in temporaryList) //For each GO in the list
            if (x.name.Contains("Enemy")) //if it's name contains "Enemy" (all the enemies, and only the enemies)
                enemies.Add(x); //Add them to the list of enemies
    }
    
    void Update()
    {
        foreach (GameObject x in enemies) //Foreach of the GameObjects in "enemies"
            if (x.activeSelf == false)  //if the enemy is dead
                deadEnemies.Add(x); //put it into the list of dead enemies
        foreach (GameObject x in deadEnemies) //Foreach of the GameObjects in "deadEnemies"
            if (x.GetComponent<NPC>().deathTimer + 10.0f < Time.time) //if the enemy has been dead for more than 10 seconds
                x.SetActive(true); //revive it
        foreach (GameObject x in deadEnemies) //Foreach of the GameObjects in "deadEnemies"
            if (x.activeSelf == true) //if the object isn't dead
                deadEnemies.Remove(x); //remove it, it isn't dead
    }
}
