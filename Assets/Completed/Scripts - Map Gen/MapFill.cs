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

    [Range(0, 10)]
    public int CaveIterations = 1;

    public int gCost;
    public int hCost;

    public List<Tile> ActiveTiles = new List<Tile>();

    private int floodNum = 0;

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

    private int fCost()
    {
        return gCost + hCost;
    }

    private void DrawMapTiles()
    {
        pool.DeactivateObject("Tile");
        pool.DeactivateObject("Blank");
        ActiveTiles.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (map[x, y])
                {
                    case 0:
                        GameObject blank = pool.GetPooledObject("Blank");
                        blank.transform.position = new Vector2(x, y);
                        blank.SetActive(true);
                        break;

                    case 1:
                        GameObject tile = pool.GetPooledObject("Tile");
                        tile.transform.position = new Vector2(x, y); //Transform.position = this + mapGenerator.x, mapGenerator.y
                        tile.SetActive(true);
                        Tile tileComponent = tile.GetComponent<Tile>();
                        ActiveTiles.Add(tileComponent);
                        tileComponent.SetPosition(x, y);
                        break;

                    case 2:

                        break;

                    case 3:

                        break;

                    default:
                        break;
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

        map = ConnectTunnels(map);

        // tempMap[x, y] = ConnectTunnels(tempMap, x, y);
        DrawMapTiles();
    }

    private void RandomFillMap()
    {
        System.Random rndSeed = new System.Random(seed);
        if (Cave)
        {
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
        else //if not cave i.e. 2D side view level
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (y < 3)
                    {
                        map[x, y] = 1;
                    }
                    if (y > height - 3)
                    {
                        map[x, y] = 0;
                    }
                    if (y > 3 && y < height - 3)
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
                    //for (int i = 0; i < CaveIterations; i++)

                    tempMap[x, y] = 1;
                }
                else
                {
                    tempMap[x, y] = 0;
                }
                //for (int i = 0; i < CaveIterations; i++)
                //tempMap[x, y] = ConnectTunnels(tempMap, x, y);
            }
        }

        return tempMap;
    }

    private int GetSurroundingObjCount(int curX, int curY)
    {
        int wallCount = 0;
        for (int neighX = curX - 1; neighX <= curX + 1; neighX++)
        {
            for (int neighY = curY - 1; neighY <= curY + 1; neighY++) //for each neighbour
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

    private int[,] ConnectTunnels(int[,] _tempMap)
    {
        int[,] digMap = new int[width, height];
        digMap = _tempMap;
        //int neighbouringWalls = GetSurroundingObjCount(curX, curY);
        for (int x = width - 1; x > 0; x--)
        {
            for (int y = height - 1; y > 0; y--)
            {
                if (map[x, y - 1] == 0)
                {
                    digMap[x, y] = 0;
                }
            }
        }
        //Floodfill Check
        FloodFill(digMap, 1, height - 1);

        return digMap;
    }

    private bool FloodFill(int[,] _map, int curX, int curY)
    {
        if (_map[curX, curY] == 0)
        {
            floodNum++; //flooded tiles incrementor
            if (curX < width)
                FloodFill(_map, curX + 1, curY);
            if (curX > 0)
                FloodFill(_map, curX - 1, curY);
            if (curY < height)
                FloodFill(_map, curX, curY + 1);
            if (curY > 0)
                FloodFill(_map, curX, curY - 1);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Tile ReturnTile(int x, int y)
    {
        foreach (Tile tile in ActiveTiles)
        {
            if (tile.TileLocation(x, y))
            {
                return tile;
            }
        }
        return null;
    }
}

/*
 //map = 1 = tile
        //map = 0 = blank
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList = pool.ReturnActiveTiles("Tile");

        //Get tile neighbours

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost() <= currentTile.fCost() && openList[i].hCost < currentTile.hCost)
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == _endPos)
            {
                return; //end found
            }

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 && y != 0) //if not this tile
                    {
                        int checkX = currentTile.localX + x; //check x neighbour
                        int checkY = currentTile.localY + y; //check y neighbour

                        if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) //if within grid bounds
                        {
                            //for each neighbour
                            if (map[checkX, checkY] == 1 || closedList.Contains(ReturnTile(checkX, checkY)))
                            {
                                //this is too complicated. jsut build fucking tunnels.
                            }
                        }
                    }
                }
            }
            //A* pathfinding
            //distance form start node + distance from end node
            /*
             open:nodes to be evaluated
             closed: nodes already evaluated

            if(tile.tag("Tile"))
            for each(Tile i)
                for each (Tile b)
             if(tile[i].Distance(this, startPos) + Tile.Distance(this,Startpos)) < tile[b].Distance(this, startPos) + Tile[b].Distance(this,Startpos))
              closedlist.Add(tile[i])
              openlist.Remove(tile[i])
              if(tile[i] == targetTile)
                return

             for each(neighbourTile)
                if(!closed)
                    if (Distance(neighbourTile, startTile) < minDist)
                        Tile shortestTile = neighbourTile
        }

     */