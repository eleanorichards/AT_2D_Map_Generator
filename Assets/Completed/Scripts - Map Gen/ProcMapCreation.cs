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

        //mapSpawn1.instantiate
        //mapSpawn2.transform = mapSawp1 + width;
    }

    //Could gen 3 at once
    //once middle of middle is reached
    //disable 1st
    //create new after 3rd

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            //DEACTIVATE all objects
            pool.DeactivateObject("Tile");
            pool.DeactivateObject("Blank");
            pool.DeactivateObject("TopTile");

            GameObject mapSpawn = pool.GetPooledObject("MapGenerator");
            mapSpawn.transform.position = new Vector2(transform.position.x - 10, transform.position.y); //setLocations + mapGeneratorPos
            mapSpawn.SetActive(true);
        }
    }
}