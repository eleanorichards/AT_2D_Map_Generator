using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public List<TileSprite> tileSprites;
    public Vector2 mapSize;
    public Sprite defaultTexture;

    public GameObject tileContainerPrefab;
    public GameObject tilePrefab;

    public Vector2 currentPos;
    public Vector2 viewPortSize;

    private TileSprite[,] _map;
    private GameObject controller;
    private GameObject _tileContainer;
    private List<GameObject> _tiles = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
        _map = new TileSprite[(int)mapSize.x, (int)mapSize.y];
        DefaultTiles();
        SetTiles();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private TileSprite FindTile(Tiles tile)
    {
        foreach (TileSprite tileSprite in tileSprites)
        {
            if (tileSprite.tileType == tile) return tileSprite;
        }
        return null;
    }

    private void DefaultTiles()
    {
        for (var y = 0; y < mapSize.y - 1; y++)
        {
            for (var x = 0; x < mapSize.x - 1; x++)
            {
                _map[x, y] = new TileSprite("null", defaultTexture, Tiles.NULL);
            }
        }
    }

    private void SetTiles()
    {
        var index = 0;
        for (var y = 0; y < mapSize.y - 1; y++)
        {
            for (var x = 0; x < mapSize.x - 1; x++)
            {
                _map[x, y] = new TileSprite(tileSprites[index].name, tileSprites[index].texture, tileSprites[index].tileType);
                index++;
                if (index > tileSprites.Count - 1) index = 0;
            }
        }
    }

    private void AddTilesToWorld()
    {
        foreach (GameObject o in _tiles)
        {
            o.SetActive(false);
        }
        _tiles.Clear();
        //OBJECT POOLING
        _tileContainer = new tileContainerPrefab;
        var tileSize = .64f;
        var viewOffsetX = viewPortSize.x / 2f;
        var viewOffsetY = viewPortSize.y / 2f;
        for (var y = -viewOffsetY; y < viewOffsetY; y++)
        {
            for (var x = -viewOffsetX; x < viewOffsetX; x++)
            {
                var tX = x * tileSize;
                var tY = y * tileSize;

                var iX = x + currentPos.x;
                var iY = y + currentPos.y;

                if (iX < 0) continue;
                if (iY < 0) continue;
                if (iX > mapSize.x - 2) continue;
                if (iY > mapSize.y - 2) continue;

                //OBJECT POOLING
                var t = Spawn(tilePrefab);

                t.transform.position = new Vector3(tX, tY, 0);
                t.transform.SetParent(_tileContainer.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x + (int)currentPos.x, (int)y + (int)currentPos.y].texture;
                _tiles.Add(t);
            }
        }
    }
}