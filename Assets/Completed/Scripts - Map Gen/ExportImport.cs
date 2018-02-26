using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class ExportImport : MonoBehaviour
{
    public InputField levelName;
    public Button exportLevel;
    private MapFill mapInfo;
    private GameData data;
    private ProcMapCreation mapCreator;

    // Use this for initialization
    private void Start()
    {
        levelName.text = "Level name...";
        mapCreator = GameObject.Find("ProcMapSpawner").GetComponent<ProcMapCreation>();
    }

    public void ExportLevel()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        int seed = mapCreator.seedNum;
        mapInfo = GetCurrentMap();
        int[,] mapCopy = mapInfo.GetMap();
        string fileName = levelName.text;

        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        else if (fileName == "")
        {
            Debug.Log("Please enter a file name");
            return;
        }

        file = File.Create(fileName);

        data = new GameData(mapCopy, fileName);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        // string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        int[,] map;
        string fileName = levelName.text;
        if (File.Exists(fileName)) file = File.OpenRead(fileName);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();
        mapInfo = GetCurrentMap();
        map = data.map;
        mapInfo.SetMap(map);
        //clear all active maps
        //draw new map
        //place player in map (without triggers?)
        string currentName = data.fileName;

        Debug.Log(data.fileName);
        Debug.Log(data.map.Length);
    }

    public MapFill GetCurrentMap()
    {
        GameObject[] mapfills = GameObject.FindGameObjectsWithTag("MapGenerator");
        foreach (GameObject map in mapfills)
        {
            MapFill mapScript = map.GetComponent<MapFill>();
            if (mapScript.seed == mapCreator.seedNum - 1)
            {
                return mapScript;
            }
        }
        return null;
    }
}