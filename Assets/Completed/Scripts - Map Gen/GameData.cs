using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[,] map;
    public string fileName;
    public float timePlayed;
    public int width;
    public int height;

    public GameData(int[,] _map, string nameStr, int _width, int _height)
    {
        width = _width;
        height = _height;
        map = _map;
        fileName = nameStr;
    }
}