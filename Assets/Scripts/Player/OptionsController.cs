using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    GameObject options;
    private void Start()
    {
        options.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (options.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                options.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                options.SetActive(true);
            }
        }
    }
}
