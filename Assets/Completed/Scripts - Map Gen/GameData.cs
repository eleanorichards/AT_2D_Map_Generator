using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[,] map;
    public string fileName;
    public float timePlayed;

    public GameData(int[,] _map, string nameStr)
    {
        map = _map;
        fileName = nameStr;
    }
}