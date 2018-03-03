using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    private GameManager _GM;
    public Text levelNameDisplay;
    private int[,] map;
    private EditorMapFill mapInfo;

    // Use this for initialization
    private void Start()
    {
        mapInfo = GameObject.Find("GameData").GetComponent<EditorMapFill>();
        _GM = GameObject.Find("Manager").GetComponent<GameManager>();
        LoadFile();
        // levelNameDisplay.text = _GM.GetCurrentLevel();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void LoadFile()
    {
        FileStream file;
        int[,] map;
        string fileName = _GM.GetCurrentLevel();
        if (File.Exists(fileName))
            file = File.OpenRead(fileName);
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