using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour, IPointerDownHandler
{
    public int levelNumb;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (levelNumb + 1 <= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(sceneBuildIndex: levelNumb + 1);
        }
    }
}
