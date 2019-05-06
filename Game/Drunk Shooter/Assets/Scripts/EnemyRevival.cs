using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevival : MonoBehaviour
{
    public List<GameObject> enemies; //List of enemies
    public List<GameObject> deadEnemies; //List of dead enemies
    private GameObject[] temporaryList;

    void Start()
    {
        temporaryList = Resources.FindObjectsOfTypeAll<GameObject>(); //Put all the GameObjects into a list
        foreach (GameObject x in temporaryList) //Foreach of the GameObjects in the list
            if (x.gameObject.name.Contains("Enemy")) //if it's name contains the word "Enemy" (hence, an enemy)
                enemies.Add(x); //add it to the list of enemies
    }
    
    void Update()
    {
        foreach (GameObject x in enemies) //Foreach of the GameObjects in the list "enemies"
            if (x.activeSelf == false) //if they're inactive
                if (enemies.Contains(x) == false) //and not already moved to the "deadEnemies" list
                    deadEnemies.Add(x); //then they must be dead, so add them to the "deadEnemies" list
        foreach (GameObject x in deadEnemies) //Foreach of the GameObjects in the list "deadEnemies"
            if (x.GetComponent<NPC>().deathTimer + 10.0f <= Time.time) //if they've been dead for more than 10 seconds
            {
                //NEED TO RESET THE TRIGGER "death"!
                x.SetActive(true); //revive them
            }
        foreach (GameObject x in deadEnemies) //Foreach of the GameObjects in the list "deadEnemies"
            if (x.activeSelf == true) //if they're not dead
                deadEnemies.Remove(x); //then they're not dead, so remove them from the list of dead enemies
    }
}
