using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    Vector2 startPos;

    [SerializeField]
    Camera cam;
    [SerializeField]
    float parallaxEffect;

    private void Start()
    {
        startPos = this.transform.localPosition;
    }

    private void Update()
    {
        float dist = cam.transform.position.x * parallaxEffect;

        this.transform.localPosition = new Vector2(startPos.x + -dist, this.transform.localPosition.y);
        this.transform.localPosition = new Vector2(Mathf.Clamp(this.transform.localPosition.x, 0, this.transform.localPosition.x), this.transform.localPosition.y);
    }
}
