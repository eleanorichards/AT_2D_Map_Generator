using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public bool shouldExpand = true;
    public int poolSize = 100;
    public string itemName = "";
}

//acces grid index:
//totalWidth*height + curwidth

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pooledObject = new List<GameObject>();
    public List<ObjectPoolItem> itemsToPool;
    public static ObjectPool SharedInstance;

    public int width = 0;
    public int height = 0;

    [Range(0, 10)]
    public int smoothIterations = 0;

    public int seed = 0;
    public bool Cave;
    private int[,] map;

    [Range(0, 100)]
    public int randomFillPercent;

    private void Awake()
    {
        SharedInstance = this;
    }

    //Create tile array
    //set inactive
    //Add to tile list
    //Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
    private void Start()
    {
        //Crate all objects in pool
        pooledObject = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObject.Add(obj);
            }
        }
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetButton("Jump"))
        {
            GenerateMap();
            Debug.Log("new map...");
        }
    }

    private void DrawMapTiles()
    {
        //Find all active tiles
        //            _tile.SetActive(false);
        //Destroy(_tile);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject tile = GetPooledObject("Tile");
                    tile.transform.position = new Vector2(x, y);
                    tile.SetActive(true);
                }
                else if (map[x, y] == 0)
                {
                    GameObject blank = GetPooledObject("Blank");
                    blank.transform.position = new Vector2(x, y);
                    blank.SetActive(true);
                }
            }
        }
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();
        for (int i = 0; i < smoothIterations; i++)
        {
            map = SmoothMap();
        }
        DrawMapTiles();
    }

    private void RandomFillMap()
    {
        System.Random rndSeed = new System.Random(seed);
        if (Cave)
        {
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = rndSeed.Next(0, 100);
                    if (map[x, y] < randomFillPercent)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
    }

    private int[,] SmoothMap()
    {
        int[,] tempMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbouringWalls = GetSurroundingObjCount(x, y);

                if (neighbouringWalls > 4)
                {
                    tempMap[x, y] = 1;
                }
                else
                {
                    tempMap[x, y] = 0;
                }
            }
        }
        return tempMap;
    }

    private int GetSurroundingObjCount(int curX, int curY)
    {
        int wallCount = 0;
        for (int neighX = curX - 1; neighX <= curX + 1; neighX++)
        {
            for (int neighY = curY - 1; neighY <= curY + 1; neighY++)
            {
                if (neighX >= 0 && neighX < width && neighY >= 0 && neighY < height) //check within bounds
                {
                    if (neighX != curX || neighY != curY)
                    {
                        wallCount += map[neighX, neighY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    //Return inactive tiles in the pool
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObject.Count; i++)
        {
            if (!pooledObject[i].activeInHierarchy && pooledObject[i].CompareTag(tag))
            {
                return pooledObject[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.CompareTag(tag))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObject.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    //OnCollisionExit (attached to player) disable tile
    public void DisableOnExit(GameObject _tile)
    {
        if (_tile.tag == "Tile")
        {
            _tile.SetActive(false);
        }
        else
        {
            Destroy(_tile);
        }
    }

    public void EnableOnEnter(GameObject _tile)
    {
        if (_tile.tag == "Tile")
        {
            //    _tile.SetActive(true);
        }
    }
}

/* else //Level gen - needs work
                {
                    if (y > height - 5)
                    {
                        GameObject blank = GetPooledObject("Blank");
                        blank.transform.position = new Vector2(x, y);
                        blank.SetActive(true);
                    }
                    else
                    {
                        map[x, y] = rndSeed.Next(0, 100);
                        if (map[x, y] < randomFillPercent)
                        {
                            GameObject tile = GetPooledObject("Tile");
                            tile.transform.position = new Vector2(x, y);
                            tile.SetActive(true);
                        }
                        else
                        {
                            GameObject blank = GetPooledObject("Blank");
                            blank.transform.position = new Vector2(x, y);
                            blank.SetActive(true);
                        }
                    }
                }*/