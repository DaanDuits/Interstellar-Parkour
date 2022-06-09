using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectmovement : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 distance;
    public Vector2 startPos;

    private void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
     void Update()
    {
        if (this.transform.position.x < startPos.x - distance.x)
        {
            speed.x = -speed.x;
            this.transform.position = new Vector2(startPos.x - distance.x, this.transform.position.y);
        }
        if (this.transform.position.x > startPos.x + distance.x)
        {
            speed.x -= speed.x * 2;
            this.transform.position = new Vector2(startPos.x + distance.x, this.transform.position.y);
        }
        if (this.transform.position.y < startPos.y - distance.y)
        {
            speed.y = -speed.y;
            this.transform.position = new Vector2(this.transform.position.x, startPos.y - distance.y);
        }
        if (this.transform.position.y > startPos.y + distance.y)
        {
            speed.y -= speed.y * 2;
            this.transform.position = new Vector2(this.transform.position.x, startPos.y + distance.y);
        }
        this.transform.position += new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime);
    }
}
