using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnconteroller : MonoBehaviour
{
    [SerializeField]
    Vector2 SpawnPoint;
    [SerializeField]
    float boundsHeight;

    [SerializeField]
    LevelSpawnPointManager spawns;

    private void Start()
    {
        spawns = GameObject.Find("Spawns").GetComponent<LevelSpawnPointManager>();
        SpawnPoint = spawns.originalSpawn;

        this.transform.position = SpawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CheckPoint")
        {
            SpawnPoint = spawns.checkpointSpawn;
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y <= boundsHeight)
            this.transform.position = SpawnPoint;
    }
}
