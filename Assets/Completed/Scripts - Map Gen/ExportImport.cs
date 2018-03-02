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
    private EditorMapFill mapInfo;
    private GameData data;
    private int[,] map;

    //private ProcMapCreation mapCreator;

    // Use this for initialization
    private void Start()
    {
        levelName.text = "Level name...";
        mapInfo = GameObject.Find("GameData").GetComponent<EditorMapFill>();
        //mapCreator = GameObject.Find("ProcMapSpawner").GetComponent<ProcMapCreation>();
    }

    public void ExportLevel()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        map = mapInfo.GetMap();
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

        data = new GameData(map, fileName, mapInfo.width, mapInfo.height);
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

        map = data.map;
        mapInfo.SetMapDimensions(data.width, data.height);
        mapInfo.SetMap(map);

        string currentName = data.fileName;

        Debug.Log(data.fileName);
        Debug.Log(data.map.Length);
    }
}