using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMapCreation : MonoBehaviour
{
    private ObjectPool pool;

    public GameObject[] MapSpawn;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        GameObject mapSpawn = pool.GetPooledObject("MapSpawn");
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
        /*
         * foreach(tileMap in activeTileMaps)
         if(leftTile)
         {
            foreach tile in tilePool
            pool.DeactivateObject("Tile");
            pool.DeactivateObject("Blank");
        }
         */
    }
}