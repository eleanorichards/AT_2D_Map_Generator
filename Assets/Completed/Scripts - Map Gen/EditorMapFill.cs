using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMapFill : MonoBehaviour
{
    public enum TileType
    {
        BLANK,
        GROUND,
        TOPGROUND
    }

    public TileType tileType;

    public int width = 0;
    public int height = 0;
    public Sprite[] tileSprites;

    private int[,] map;
    private GameObject tile;
    private ObjectPool pool;

    // Use this for initialization
    private void Start()
    {
        pool = GameObject.Find("TilePooler").GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void GenerateMap()
    {
        map = new int[width, height];

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
}