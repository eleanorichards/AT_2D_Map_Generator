using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMapCreation : MonoBehaviour
{
    private ObjectPool pool;
    public int seedNum = 0;
    private int mapIndex = 0;
    private List<GameObject> objList = new List<GameObject>();

    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject newColliderPrefab;

    private int spawnPos = 60;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        objList.Add(GameObject.Find("SpawnPoint"));
    }

    //Could gen 3 at once
    //once middle of middle is reached
    //disable 1st
    //create new after 3rd

    public void SpawnEntered(int SpawnID)
    {
        Vector3 spawnVector = new Vector3(spawnPos, 0, 0);
        //Create next collider in sequence (+ width which is currently 60)
        GameObject spawnCollider = Instantiate(newColliderPrefab, spawnVector, Quaternion.identity);
        spawnCollider.transform.SetParent(this.gameObject.transform);
        spawnPoints.Add(spawnCollider);
        spawnCollider.GetComponent<SpawnPoint>().SetID(spawnPoints.Count);
        // spawnCollider.transform.position = spawnPoints[spawnPoints.Count - 1].transform.position + new Vector3(60, 0, 0);

        objList.Clear();
        GameObject mapSpawn = pool.GetPooledObject("MapGenerator");
        if (!mapSpawn)
            return;
        MapFill mapfill = mapSpawn.GetComponent<MapFill>();
        mapfill.seed = seedNum;
        mapfill.spawnIndex = mapIndex;
        mapSpawn.transform.position = new Vector3(spawnPoints[SpawnID - 1].transform.position.x,
            spawnPoints[SpawnID - 1].transform.position.y, 0.0f);
        mapSpawn.SetActive(true);
        objList = pool.ReturnActiveObjects("MapGenerator");

        foreach (GameObject map in objList)
        {
            if (map.GetComponent<MapFill>().spawnIndex == SpawnID - 3)
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
        spawnPos += 60;
        seedNum++;
        mapIndex++;
    }
}