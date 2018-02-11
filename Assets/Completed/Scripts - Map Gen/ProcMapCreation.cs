using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMapCreation : MonoBehaviour
{
    private ObjectPool pool;

    public GameObject[] spawnPoints;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].GetComponent<SpawnPoint>().SpawnID = i;
        }
    }

    //Could gen 3 at once
    //once middle of middle is reached
    //disable 1st
    //create new after 3rd

    // Update is called once per frame
    private void Update()
    {
    }

    public void SpawnEntered(int SpawnID)
    {
        //DEACTIVATE all objects
        pool.DeactivateObject("Tile");
        pool.DeactivateObject("Blank");
        pool.DeactivateObject("TopTile");
        pool.DeactivateObject("MapGenerator");

        GameObject mapSpawn = pool.GetPooledObject("MapGenerator");

        mapSpawn.transform.position = new Vector2(spawnPoints[SpawnID].transform.position.x,
            spawnPoints[SpawnID].transform.position.y);

        mapSpawn.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
    }
}