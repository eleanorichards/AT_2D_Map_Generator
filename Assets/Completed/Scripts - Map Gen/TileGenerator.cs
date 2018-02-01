using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    private GameObject cam = null;
    private int width = 10;
    private int height = 10;
    public GameObject tilePrefab = null;
    public List<GameObject> tiles = new List<GameObject>();

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        //PlaceGrid();
    }

    public void PlaceGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = ObjectPool.SharedInstance.GetPooledObject("Tile");
                if (obj != null)
                {
                    obj.transform.position = new Vector2(x, y);
                    obj.SetActive(true);
                }
                //tiles[x * y].transform.position = new Vector2(x, y);
            }
        }
    }

    private Vector2 worldToScreen(int x, int y)
    {
        return new Vector2(x - cam.transform.position.x, y: y - cam.transform.position.y);
    }

    private Vector2 screenToWorld(int x, int y)
    {
        return new Vector2(x + cam.transform.position.x, y + cam.transform.position.y);
    }
}