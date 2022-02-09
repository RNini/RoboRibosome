using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Scene currentScene;
    public string sceneName;


    // Start is called before the first frame update
    void Start()
    {
        // Outputs scene name connected to interacted object
        Scene currentScene = gameObject.scene;
        sceneName = currentScene.name;

    }

    // Function for determing scene change
    public void sceneChange()
    {
        if (sceneName == "1. IntroScreen")
        {
            SceneManager.LoadScene("2. CharacterSelection");
        }

        else if (sceneName == "2. CharacterSelection")
        {
            SceneManager.LoadScene("3. Tutorial");
        }

        else if (sceneName == "3. Tutorial")
        {
            SceneManager.LoadScene("4. MainGame");
        }

        else if (sceneName == "4. MainGame")
        {
            SceneManager.LoadScene("5. HighscoreMenu");
        }

    }

    public void restartGame()
    {
        SceneManager.LoadScene("4. MainGame");
    }

    public void return2Menu()
    {
        SceneManager.LoadScene("1. IntroScreen");
    }
}
