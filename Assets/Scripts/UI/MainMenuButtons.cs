using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void SartGame()
    {
        SceneManager.LoadScene(sceneName:"LevelSelector");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToMap()
    {
        SceneManager.LoadScene(1);
    }
}
