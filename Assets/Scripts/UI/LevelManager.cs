using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int skippedObjects;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.GetComponent<LevelSelect>() != null && this.transform.GetChild(i).gameObject.name == "level")
            {
                this.transform.GetChild(i).gameObject.GetComponent<LevelSelect>().levelNumb = i + 1 - skippedObjects;
            }
            else
            {
                skippedObjects++;
            }
        }
    }
}
