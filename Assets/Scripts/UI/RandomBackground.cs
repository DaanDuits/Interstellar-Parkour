using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackground : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    void Start()
    {
        System.Random prng = new System.Random();

        int rand = prng.Next(0, sprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = sprites[rand];
    }
}
