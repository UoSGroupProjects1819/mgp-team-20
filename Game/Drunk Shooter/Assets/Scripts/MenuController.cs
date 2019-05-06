using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Button startButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(delegate { SceneManager.LoadScene(sceneName: "Level_Bar");
        });

        exitButton.onClick.AddListener(delegate {
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
