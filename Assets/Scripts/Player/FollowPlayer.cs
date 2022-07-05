using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float minX;
    [SerializeField]
    float maxX;

    void Update()
    {
        Vector3 pos = new Vector3(player.position.x, player.position.y, -2);

        this.transform.position = new Vector3(pos.x, pos.y, pos.z);
        Vector3 oldPos = this.transform.position;
        this.transform.position = new Vector3(Mathf.Clamp(oldPos.x, minX, maxX), pos.y, pos.z);
    }
}
