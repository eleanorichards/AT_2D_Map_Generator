using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMapCreation : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //Could gen 3 at once
        //once middle of middle is reached
        //disable 1st
        //create new after 3rd
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
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