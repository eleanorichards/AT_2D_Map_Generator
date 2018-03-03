using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFill : MonoBehaviour
{
    public enum TileType
    {
        BLANK,
        GROUND,
        TOPGROUND
    }

    public TileType tileType;

    private ObjectPool pool;
    public int width = 0;
    public int height = 0;

    [Range(0, 10)]
    public int smoothIterations = 0;

    public int seed = 0;
    public int spawnIndex = 0;
    public bool Cave;
    private int[,] map;

    [Range(0, 90)]
    public float randomFillPercent;

    private int floodNum = 0;
    private GameObject tile;
    public Sprite[] tileSprites;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        floodNum = 0;

        Invoke("GenerateMap", 0.0f);
        Debug.Log("new map..." + seed);
    }

    public void Dropdown_tileTypeChanged(int selected)
    {
        switch (selected)
        {
            case 0:
                tileType = TileType.GROUND;
                break;

            case 1:
                tileType = TileType.TOPGROUND;

                break;

            case 2:
                tileType = TileType.BLANK;
                break;

            default:
                break;
        }
    }

    private void DrawMapTiles()
    {
        //DeActivate all active tiles from pool
        if (!pool)
        {
            return;
        }
        // pool.DeactivateObject("Tile");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (map[x, y])
                {
                    case -1:
                        break;

                    case 0:
                        break;

                    case 1: //Ground Tile
                        tile = pool.GetPooledObject("Tile");
                        tile.transform.position = new Vector2(x, y) + new Vector2(transform.position.x, transform.position.y); //setLocations + mapGeneratorPos
                        tile.SetActive(true);
                        //Sprite switching
                        int i = Random.Range(0, 3);
                        tile.GetComponent<SpriteRenderer>().sprite = tileSprites[i];
                        tile.transform.SetParent(this.transform); //Set this map fill obj as parent
                        break;

                    case 2://Top tile
                        tile = pool.GetPooledObject("Tile");
                        tile.transform.position = new Vector2(x, y) + new Vector2(transform.position.x, transform.position.y); //setLocations + mapGeneratorPos
                        tile.SetActive(true);
                        i = Random.Range(4, 6);
                        tile.GetComponent<SpriteRenderer>().sprite = tileSprites[4];
                        tile.transform.SetParent(this.transform); //Set this map fill obj as parent
                        break;

                    case 3:
                        //Treasure
                        break;

                    case 4:
                        break;

                    case 5:
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void DeactivateTile(int _x, int _y)
    {
        List<GameObject> tiles = new List<GameObject>();
        tiles = pool.ReturnActiveObjects("Tile");

        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position == new Vector3(_x, _y, 0))
            {
                tile.SetActive(false);
                // map[_x, _y] == tileType
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
        float noOfBlankTiles = (width * height) * (randomFillPercent / 100.0f);
        while (FloodFill(map, 1, height - 2) <= noOfBlankTiles) //IF FLOODFILL returns less than no of blank tiles
        {
            Debug.Log(floodNum);
            map = ConnectTunnels(map);
        }
        TextureTiles(map);
        DrawMapTiles();
    }

    public void UserMapEdit(int _x, int _y)
    {
        List<GameObject> tiles = new List<GameObject>();
        tiles = pool.ReturnActiveObjects("Tile");

        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position == new Vector3(_x, _y, 0))
            {
                tile.SetActive(false);
            }
        }

        switch (tileType)
        {
            case TileType.BLANK:
                map[_x, _y] = 0;
                break;

            case TileType.GROUND:
                map[_x, _y] = 1;
                break;

            case TileType.TOPGROUND:
                map[_x, _y] = 2;
                break;

            default:
                break;
        }
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
        for (int x = width - 2; x > 0; x--)
        {
            for (int y = height - 2; y > 0; y--)
            {
                if (map[x, y] == -1)
                {
                    map[x, y] = 0;
                    //RESET anything changed by floodfill
                }
                if (map[x, y] > 0) //If solid
                {
                    if (map[x, y - 1] == 0) //if below is blank
                    {
                        digMap[x, y] = 0;

                        if (map[x, y + 1] == 1)
                        {
                            digMap[x, y + 1] = 0;
                        }
                    }
                }
            }
        }

        return digMap;
    }

    private int FloodFill(int[,] _map, int curX, int curY)
    {
        if (curX < 0 || curX >= width || curY < 0 || curY >= height) //Bounds check
        {
            return 0;
        }

        if (_map[curX, curY] == -1)
            return 0;
        if (_map[curX, curY] != 0)
            return 0;

        _map[curX, curY] = -1;   //set to replace int
        floodNum++;             //increment flooded num of tiles

        FloodFill(_map, curX, curY + 1);

        FloodFill(_map, curX - 1, curY);

        FloodFill(_map, curX + 1, curY);

        FloodFill(_map, curX, curY - 1);

        return floodNum;
    }

    private void TextureTiles(int[,] _map)
    {
        //0 - BLANK
        //1 - STANDARD DIRT
        //2 - TOP TILE
        //3
        //4
        //5
        //6
        //7
        //8
        //9
        //10
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_map[x, y] == 1)
                {
                    if (y < height - 1)
                    {
                        if (_map[x, y + 1] <= 0) //blank
                        {
                            map[x, y] = 2;
                        }
                    }
                }
            }
        }
    }
}

/* DISCARDED A* ALGORITHM
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