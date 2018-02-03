using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFill : MonoBehaviour
{
    private ObjectPool pool;
    public int width = 0;
    public int height = 0;

    [Range(0, 10)]
    public int smoothIterations = 0;

    public int seed = 0;
    public bool Cave;
    private int[,] map;

    [Range(0, 100)]
    public int randomFillPercent;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
        //GenerateMap();
    }

    // Update is called once per frame
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
        //_tile.SetActive(false);
        ///Destroy(_tile);
        pool.DeactivateObject("Tile");
        pool.DeactivateObject("Blank");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject tile = pool.GetPooledObject("Tile");
                    tile.transform.position = new Vector2(x, y);
                    tile.SetActive(true);
                }
                else if (map[x, y] == 0)
                {
                    GameObject blank = pool.GetPooledObject("Blank");
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
}