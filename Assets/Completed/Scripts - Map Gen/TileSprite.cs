using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TileSprite : MonoBehaviour
{
    public string name;
    public Sprite texture;
    public Tiles tileType;

    // Use this for initialization
    private void Start()
    {
        name = "null";
        texture = new Sprite();
        tileType = Tiles.NULL;
    }

    public TileSprite(string _name, Sprite _texture, Tiles _tileType)
    {
        name = _name;
        texture = _texture;
        tileType = _tileType;
    }
}