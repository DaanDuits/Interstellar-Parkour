using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlanetRotation : MonoBehaviour
{
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }
}
