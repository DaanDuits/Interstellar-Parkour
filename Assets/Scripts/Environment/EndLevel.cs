using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField]
    float waitSeconds;

    [SerializeField]
    GameObject victoryText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
        {
            StartCoroutine(WaitAndEnd());
            collision.transform.GetChild(0).gameObject.SetActive(true);
            victoryText.SetActive(true);
        }
    }

    IEnumerator WaitAndEnd()
    {
        yield return new WaitForSeconds(waitSeconds);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }
}
