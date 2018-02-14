using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMapCreation : MonoBehaviour
{
    private ObjectPool pool;
    private int seedNum = 0;
    private int mapIndex = 0;

    public GameObject[] spawnPoints;
    private List<GameObject> objList = new List<GameObject>();

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
        objList.Clear();
        GameObject mapSpawn = pool.GetPooledObject("MapGenerator");
        if (!mapSpawn)
            return;
        MapFill mapfill = mapSpawn.GetComponent<MapFill>();
        mapfill.seed = seedNum;
        mapfill.spawnIndex = mapIndex;
        mapSpawn.transform.position = new Vector3(spawnPoints[SpawnID].transform.position.x,
            spawnPoints[SpawnID].transform.position.y, 0.0f);
        mapSpawn.SetActive(true);
        objList = pool.ReturnActiveObjects("MapGenerator");

        foreach (GameObject map in objList)
        {
            if (map.GetComponent<MapFill>().spawnIndex == mapIndex - 3)
            {
                map.SetActive(false);
            }
        }
        //  if (objList.Count > 2)
        // {
        //DEACTIVATE all objects
        //    objList[0].gameObject.SetActive(false);
        // objList.RemoveAt(0);
        //  }
        // objList.Clear();
        // objList = pool.ReturnActiveObjects("MapGenerator");
        seedNum++;
        mapIndex++;
    }
}